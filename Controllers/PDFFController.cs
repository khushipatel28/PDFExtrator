using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdf1.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using pdf1.Data;

namespace pdf1.Controllers
{
    public class PDFFController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDbContext _context;

        public PDFFController(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(PdfModel model)
        {
            if (model.PdfFile != null && model.PdfFile.Length > 0)
            {
                StringBuilder sb = new StringBuilder();

                using (var ms = new MemoryStream())


                {
                    model.PdfFile.CopyTo(ms);
                    byte[] fileBytes = ms.ToArray();

                    using (PdfReader reader = new PdfReader(new MemoryStream(fileBytes)))
                    {
                        for (int pageNo = 1; pageNo <= reader.NumberOfPages; pageNo++)
                        {
                            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                            string text = PdfTextExtractor.GetTextFromPage(reader, pageNo, strategy);
                            text = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(text)));
                            sb.Append(text);
                        }
                    }
                }

                model.ExtractedText = sb.ToString();
                model.ApiResponse = await GetCompletionFromOpenAI("OPENAI_API_KEY", model.ExtractedText);

                var resourceDetail = await SaveDataToDatabase(model.ApiResponse);

                return RedirectToAction("List");
               // return RedirectToAction("GetItem");
            }

            //var resourceDetail = await SaveDataToDatabase(model.ApiResponse);

            //return View("List", resourceDetail);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFromUrl(string pdfUrl)
        {
            if (!string.IsNullOrEmpty(pdfUrl))
            {
                StringBuilder sb = new StringBuilder();

                using (var client = _httpClientFactory.CreateClient())
                {
                    var response = await client.GetAsync(pdfUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var fileBytes = await response.Content.ReadAsByteArrayAsync();

                        using (PdfReader reader = new PdfReader(new MemoryStream(fileBytes)))
                        {
                            for (int pageNo = 1; pageNo <= reader.NumberOfPages; pageNo++)
                            {
                                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                                string text = PdfTextExtractor.GetTextFromPage(reader, pageNo, strategy);
                                text = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(text)));
                                sb.Append(text);
                            }
                        }

                        var model = new PdfModel
                        {
                            ExtractedText = sb.ToString(),
                            ApiResponse = await GetCompletionFromOpenAI("OPENAI_API_KEY", sb.ToString())
                        };

                        var resourceDetail = await SaveDataToDatabase(model.ApiResponse);

                        return RedirectToAction("List");
                    }
                }
            }

            return View("Index");
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            var resourceDetails = await _context.ResourceDetails
                .Include(rd => rd.edu)
                .Include(rd => rd.exp)
                .Include(rd => rd.pro)
                .Include(rd => rd.cer)
                .ToListAsync();

            return View(resourceDetails);
        }

        //[HttpGet]
        //public async Task<IActionResult> List()
        //{
        //    var resourceDetails = await _context.ResourceDetails
        //        .Include(rd => rd.ski)
        //        .ToListAsync();

        //    return View(resourceDetails);
        //}

        [HttpGet]
        public async Task<IActionResult> DetailsPartial(int id)
        {
            var resourceDetail = await _context.ResourceDetails
                .Include(rd => rd.edu)
                .Include(rd => rd.exp)
                .Include(rd => rd.pro)
                .Include(rd => rd.cer)
                .Include(rd => rd.ski)
                .FirstOrDefaultAsync(rd => rd.ResourceId == id);

            if (resourceDetail == null)
            {
                return NotFound();
            }

            return PartialView("_DetailsPartial", resourceDetail);
        }

        private async Task<string> GetCompletionFromOpenAI(string apiKey, string prompt)
        {
            var client = _httpClientFactory.CreateClient();

            // Configure the request
            var request = new
            {
                model = "gpt-3.5-turbo-0125",
                response_format = new { type = "json_object" },
                messages = new[]
                {
                    new { role = "system", content = "You are a Resume Parser" },
                    new { role = "user", content = "I will give you a resume and you will give me info in json format in the following json structure: First name, Last name, Email, Phone number, Country, State, City, Zip code, Address line 1, Address line 2, Linkedin url, Years of experience, About me, Skills (array), Education (array): -> University name -> Degree -> Course -> Start month/year -> End month/year -> Country -> Description, Experience (array): -> Company name -> Designation -> Start month/year -> End month/year -> Description, Project (array): -> Project name -> Designation -> Start month/year -> End month/year -> Description, Certificate (array): -> Name -> Provider -> Date -> Certificate url -> Description; for date keep format month/year and dont add anything else. if you dont have then keep it blank or if currently working then keep present. dont write multiple skills together. do not miss anything, especially in description of experience. output should only be json" },
                    new { role = "assistant", content = "Sure, please provide the resume, and I'll format the information in JSON format for you." },
                    new { role = "user", content = prompt }
                }
            };

            // Serialize the request data
            var jsonRequest = JsonSerializer.Serialize(request);

            // Set up the request headers
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "OPENAI_API_KEY");

            //var response = await client.PostAsync("https://api.openai.com/v1/chat/completions",
            //    new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
            //var jsonResponse = await response.Content.ReadAsStringAsync();
            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", new StringContent(jsonRequest, Encoding.UTF8, "application/json"));
            //var jsonResponse = await response.Content.ReadAsStringAsync();

            var jsonResponse = "{\r\n  \"id\": \"chatcmpl-9QvJXC8ZQsqPJiz2yoSkOVwnnyrKM\",\r\n  \"object\": \"chat.completion\",\r\n  \"created\": 1716203871,\r\n  \"model\": \"gpt-3.5-turbo-0125\",\r\n  \"choices\": [\r\n    {\r\n      \"index\": 0,\r\n      \"message\": {\r\n        \"role\": \"assistant\",\r\n        \"content\": \"{\\n  \\\"First name\\\": \\\"Yash\\\",\\n  \\\"Last name\\\": \\\"Dhamecha\\\",\\n  \\\"Email\\\": \\\"yashdhamecha55@gmail.com\\\",\\n  \\\"Phone number\\\": \\\"+91 8000633700\\\",\\n  \\\"Country\\\": \\\"India\\\",\\n  \\\"State\\\": \\\"\\\",\\n  \\\"City\\\": \\\"\\\",\\n  \\\"Zip code\\\": \\\"\\\",\\n  \\\"Address line 1\\\": \\\"\\\",\\n  \\\"Address line 2\\\": \\\"\\\",\\n  \\\"Linkedin url\\\": \\\"https://www.linkedin.com/in/ca-yash-dhamecha-9829ab160\\\",\\n  \\\"Years of experience\\\": \\\"4\\\",\\n  \\\"About me\\\": \\\"To Work in a dynamic environment that provides me a wide spectrum of experience and exposure. To bring a versatile portfolio of skills at workplace and serve the organization with positive attitude and efficiency.\\\",\\n  \\\"Skills\\\": [\\n    \\\"Microsoft ERP (Dynamics 365)\\\",\\n    \\\"Project scope definition\\\",\\n    \\\"Process analysis\\\",\\n    \\\"Financial impact analysis\\\",\\n    \\\"Business process improvement\\\"\\n  ],\\n  \\\"Education\\\": [\\n    {\\n      \\\"University name\\\": \\\"Institute of Chartered Accountant of India\\\",\\n      \\\"Degree\\\": \\\"Chartered Accountant\\\",\\n      \\\"Course\\\": \\\"\\\",\\n      \\\"Start month/year\\\": \\\"01/2013\\\",\\n      \\\"End month/year\\\": \\\"06/2017\\\",\\n      \\\"Country\\\": \\\"India\\\",\\n      \\\"Description\\\": \\\"Cleared entire CA throughout.\\\"\\n    },\\n    {\\n      \\\"University name\\\": \\\"Gujarat University\\\",\\n      \\\"Degree\\\": \\\"Bachelor of Commerce\\\",\\n      \\\"Course\\\": \\\"\\\",\\n      \\\"Start month/year\\\": \\\"07/2014\\\",\\n      \\\"End month/year\\\": \\\"06/2018\\\",\\n      \\\"Country\\\": \\\"India\\\",\\n      \\\"Description\\\": \\\"\\\"\\n    }\\n  ],\\n  \\\"Experience\\\": [\\n    {\\n      \\\"Company name\\\": \\\"Larson & Toubro Infotech Ltd.\\\",\\n      \\\"Designation\\\": \\\"Senior consultant\\\",\\n      \\\"Start month/year\\\": \\\"11/2021\\\",\\n      \\\"End month/year\\\": \\\"Present\\\",\\n      \\\"Description\\\": \\\"Independently handled Finance related Modules in end to end Implementation. Analyzed the on-going business process and guided them to improve existing process to the best practices. Prepared Functional Documents, Implementation Setup, Testing, Finance Support and deliverables. Provided training of concerned modules to user and business owners.\\\"\\n    },\\n    {\\n      \\\"Company name\\\": \\\"Ernst & Young LLP\\\",\\n      \\\"Designation\\\": \\\"Consultant\\\",\\n      \\\"Start month/year\\\": \\\"03/2021\\\",\\n      \\\"End month/year\\\": \\\"11/2021\\\",\\n      \\\"Description\\\": \\\"Responsible for Go-live activities and hyper care support. Worked on Accounts Payable, Accounts Receivable, Project Accounting, Purchase & Sales, General Ledger, and Fixed Asset modules.\\\"\\n    },\\n    {\\n      \\\"Company name\\\": \\\"Intech Systems Pvt. Ltd.\\\",\\n      \\\"Designation\\\": \\\"Sr. Business Analyst\\\",\\n      \\\"Start month/year\\\": \\\"05/2018\\\",\\n      \\\"End month/year\\\": \\\"03/2021\\\",\\n      \\\"Description\\\": \\\"Lead end-to-end implementation projects in the packaging and heavy engineering industries. Worked on Accounts Payable, Accounts Receivable, Inventory Management, Purchase & Sales, General Ledger, Fixed Asset, Project Accounting, and Expense Management modules.\\\"\\n    }\\n  ],\\n  \\\"Project\\\": [\\n    {\\n      \\\"Project name\\\": \\\"Larson & Toubro Infotech Ltd.\\\",\\n      \\\"Designation\\\": \\\"Senior consultant\\\",\\n      \\\"Start month/year\\\": \\\"\\\",\\n      \\\"End month/year\\\": \\\"Present\\\",\\n      \\\"Description\\\": \\\"Project Industry: Mobile computing accessories. Project Type: Assessment and Implementation. Modules: Accounts Payable, Accounts Receivable, Project accounting, Purchase & Sales, General Ledger, Fixed Asset.\\\"\\n    },\\n    {\\n      \\\"Project name\\\": \\\"Ernst & Young LLP\\\",\\n      \\\"Designation\\\": \\\"Consultant\\\",\\n      \\\"Start month/year\\\": \\\"\\\",\\n      \\\"End month/year\\\": \\\"Present\\\",\\n      \\\"Description\\\": \\\"Project Industry: Professional software services. Project Type: Go-live activities and hyper care support. Modules: Accounts Payable, Accounts Receivable, Project Accounting, Purchase & Sales, General Ledger, Fixed Asset.\\\"\\n    },\\n    {\\n      \\\"Project name\\\": \\\"Intech Systems Pvt. Ltd.\\\",\\n      \\\"Designation\\\": \\\"Sr. Business Analyst\\\",\\n      \\\"Start month/year\\\": \\\"\\\",\\n      \\\"End month/year\\\": \\\"Present\\\",\\n      \\\"Description\\\": \\\"Project Industry: Packaging industry and Heavy Engineering. Project Type: End-to-end implementation. Modules implemented: Accounts Payable, Accounts Receivable, Inventory management, Purchase & Sales, General Ledger, Fixed Asset, Project accounting, Expense Management.\\\"\\n    }\\n  ],\\n  \\\"Certificate\\\": [\\n    {\\n      \\\"Name\\\": \\\"MB 300\\\",\\n      \\\"Provider\\\": \\\"Microsoft\\\",\\n      \\\"Date\\\": \\\"Present\\\",\\n      \\\"Certificate url\\\": \\\"\\\",\\n      \\\"Description\\\": \\\"Microsoft Dynamics 365 Core Finance and Operations\\\"\\n    },\\n    {\\n      \\\"Name\\\": \\\"MB 310\\\",\\n      \\\"Provider\\\": \\\"Microsoft\\\",\\n      \\\"Date\\\": \\\"Present\\\",\\n      \\\"Certificate url\\\": \\\"\\\",\\n      \\\"Description\\\": \\\"Microsoft Dynamics 365 Finance Functional Consultant\\\"\\n    }\\n  ]\\n}\"\r\n      },\r\n      \"logprobs\": null,\r\n      \"finish_reason\": \"stop\"\r\n    }\r\n  ],\r\n  \"usage\": {\r\n    \"prompt_tokens\": 1484,\r\n    \"completion_tokens\": 1046,\r\n    \"total_tokens\": 2530\r\n  },\r\n  \"system_fingerprint\": null\r\n}";
            //dynamic responseObject = System.Text.Json.JsonSerializer.Deserialize<dynamic>(jsonResponse);
            //string assistantContent = responseObject.GetProperty("choices")[0].GetProperty("message").GetProperty("content");

            var responseObject = JsonSerializer.Deserialize<JsonElement>(jsonResponse);
            var content = responseObject.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            return content;

            //using (var context = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()))
            //{
            //    var resourceDetail = new ResourceDetail
            //    {
            //        //Id = Guid.NewGuid(),
            //        FirstName = GetPropertyValue(data, "First name"),
            //        LastName = GetPropertyValue(data, "Last name"),
            //        Email = GetPropertyValue(data, "Email"),
            //        PhoneNumber = GetPropertyValue(data, "Phone number"),
            //        Country = GetPropertyValue(data, "Country"),
            //        State = GetPropertyValue(data, "State"),
            //        City = GetPropertyValue(data, "City"),
            //        ZipCode = GetPropertyValue(data, "Zip code"),
            //        AddressLine1 = GetPropertyValue(data, "Address line 1"),
            //        AddressLine2 = GetPropertyValue(data, "Address line 2"),
            //        LinkedInUrl = GetPropertyValue(data, "Linkedin url"),
            //        YearOfExperience = GetPropertyValue(data, "Years of experience"),
            //        AboutMe = GetPropertyValue(data, "About me")
            //    };

            //   // _context.ResourceDetails.Add(resourceDetail);
            //   // _context.SaveChanges();


            //    var eduction = new Education
            //{
            //    //Education_Id = Guid.NewGuid(),
            //    UniversityName = GetPropertyValue(data, "University name"),
            //    Degree = GetPropertyValue(data, "Degree"),
            //    Course = GetPropertyValue(data, "Course"),
            //    StartMonthYear = GetPropertyValue(data, "Start month/year"),
            //    EndMonthYear = GetPropertyValue(data, "End month/year"),
            //    Country = GetPropertyValue(data, "Country"),
            //    Description = GetPropertyValue(data, "Description"),

            //};

            //var experience = new Experience
            //{
            //    //Experience_Id = Guid.NewGuid(),
            //    CompanyName = GetPropertyValue(data, "Company name"),
            //    Designation = GetPropertyValue(data, "Designation"),
            //    StartDate = GetPropertyValue(data, "Start month/year"),
            //    EndDate = GetPropertyValue(data, "End month/year"),
            //    Description = GetPropertyValue(data, "Description"),
            //};

            //var project = new Project
            //{
            //    //Project_Id = Guid.NewGuid(),
            //    ProjectName = GetPropertyValue(data, "Project Name"),
            //    Designation = GetPropertyValue(data, "Designation"),
            //    StartDate = GetPropertyValue(data, "Start month/year"),
            //    EndDate = GetPropertyValue(data, "End month/year"),
            //    Description = GetPropertyValue(data, "Description"),
            //};

            //var certificates = new Certificate
            //{
            //    //Certificate_Id = Guid.NewGuid(),
            //    Name = GetPropertyValue(data, "Name"),
            //    Provider = GetPropertyValue(data, "Provider"),
            //    Date = GetPropertyValue(data, "Date"),
            //    CertificateUrl = GetPropertyValue(data, "Certificate Url"),
            //    Description = GetPropertyValue(data, "Description"),
            //};

            //_context.ResourceDetails.Add(resourceDetail);
            //_context.Educations.Add(eduction);
            //_context.Experiences.Add(experience);
            //_context.Projects.Add(project);
            //_context.Certificates.Add(certificates);
            //_context.SaveChanges();
            //return "";
        }


        // Return the content as an ObjectResult


        private async Task<ResourceDetail> SaveDataToDatabase(string apiResponse)
        {
            var data = JsonSerializer.Deserialize<JsonElement>(apiResponse);

            var resourceDetail = new ResourceDetail
            {
                FirstName = GetPropertyValue(data, "First name"),
                LastName = GetPropertyValue(data, "Last name"),
                Email = GetPropertyValue(data, "Email"),
                PhoneNumber = GetPropertyValue(data, "Phone number"),
                Country = GetPropertyValue(data, "Country"),
                State = GetPropertyValue(data, "State"),
                City = GetPropertyValue(data, "City"),
                ZipCode = GetPropertyValue(data, "Zip code"),
                AddressLine1 = GetPropertyValue(data, "Address line 1"),
                AddressLine2 = GetPropertyValue(data, "Address line 2"),
                LinkedInUrl = GetPropertyValue(data, "Linkedin url"),
                YearOfExperience = GetPropertyValue(data, "Years of experience"),
                AboutMe = GetPropertyValue(data, "About me")
                
            };

            //_context.ResourceDetails.Add(resourceDetail);
            //await _context.SaveChangesAsync();

            //foreach (var skillDescription in selectedSkills)
            //{
            //    var skill = new Skill
            //    {
            //        Skills = skillDescription,
            //        res = resourceDetail
            //    };
            //    resourceDetail.ski.Add(skill);
            //}

            if (data.TryGetProperty("Skills", out JsonElement skillsElement))
            {
                foreach (var skillElement in skillsElement.EnumerateArray())
                {
                    var skill = new Skill
                    {
                        Skills = skillElement.GetString(),
                    };
                    resourceDetail.ski.Add(skill);
                }
            }

            //foreach (var skillId in data.GetProperty("Skill").EnumerateArray())
            //{
            //    var skill = new Skill
            //    {
            //        Skills = GetPropertyValue(skillId, "Skills"),
            //        Id = resourceDetail.Id,// Assuming ResourceDetail has a primary key Id
            //        res = resourceDetail
            //    };
            //    resourceDetail.ski.Add(skill);
            //}

            foreach (var educationElement in data.GetProperty("Education").EnumerateArray())
            {
                var education = new Education
                {
                    UniversityName = GetPropertyValue(educationElement, "University name"),
                    Degree = GetPropertyValue(educationElement, "Degree"),
                    Course = GetPropertyValue(educationElement, "Course"),
                    StartMonthYear = GetPropertyValue(educationElement, "Start month/year"),
                    EndMonthYear = GetPropertyValue(educationElement, "End month/year"),
                    Country = GetPropertyValue(educationElement, "Country"),
                    Description = GetPropertyValue(educationElement, "Description"),
                    Id = resourceDetail.ResourceId ,// Assuming ResourceDetail has a primary key Id
                    res = resourceDetail

                };
                resourceDetail.edu.Add(education);
            }

            foreach (var experienceElement in data.GetProperty("Experience").EnumerateArray())
            {
                var experience = new Experience
                {
                    CompanyName = GetPropertyValue(experienceElement, "Company name"),
                    Designation = GetPropertyValue(experienceElement, "Designation"),
                    StartDate = GetPropertyValue(experienceElement, "Start month/year"),
                    EndDate = GetPropertyValue(experienceElement, "End month/year"),
                    Description = GetPropertyValue(experienceElement, "Description"),
                    Id = resourceDetail.ResourceId,
                    res = resourceDetail
                };
                resourceDetail.exp.Add(experience);
            }

            foreach (var projectElement in data.GetProperty("Project").EnumerateArray())
            {
                var project = new Project
                {
                    ProjectName = GetPropertyValue(projectElement, "Project name"),
                    Designation = GetPropertyValue(projectElement, "Designation"),
                    StartDate = GetPropertyValue(projectElement, "Start month/year"),
                    EndDate = GetPropertyValue(projectElement, "End month/year"),
                    Description = GetPropertyValue(projectElement, "Description"),
                    Id = resourceDetail.ResourceId,
                    res = resourceDetail
                };
                resourceDetail.pro.Add(project);
            }

            foreach (var certificateElement in data.GetProperty("Certificate").EnumerateArray())
            {
                var certificate = new Certificate
                {
                    Name = GetPropertyValue(certificateElement, "Name"),
                    Provider = GetPropertyValue(certificateElement, "Provider"),
                    Date = GetPropertyValue(certificateElement, "Date"),
                    CertificateUrl = GetPropertyValue(certificateElement, "Certificate url"),
                    Description = GetPropertyValue(certificateElement, "Description"),
                    Id = resourceDetail.ResourceId,
                    res = resourceDetail
                };
                resourceDetail.cer.Add(certificate);
            }

            _context.ResourceDetails.Add(resourceDetail);
            await _context.SaveChangesAsync();
            return resourceDetail;
            //return "Data saved successfully"; // Return a success message or relevant string
        }


        //[HttpPost]
        //public IActionResult GetItem(int[] SkillsIds)
        //{
        //    var resourceDetails = _context.ResourceDetails
        //        .Where(rd => rd.ski.Any(s => SkillsIds.Contains(s.SId)))
        //        .Include(rd => rd.ski)
        //        .ToList();

        //    return PartialView("_INFO", resourceDetails);
        //}

        [HttpPost]
        public IActionResult GetItem(int[] SkillsIds)
        {
            var resourceDetails = _context.ResourceDetails
                .Where(rd => rd.ski.Any(s => SkillsIds.Contains(s.SId)))
                .ToList();

            return PartialView("_INFO", resourceDetails);
        }





        private string GetPropertyValue(JsonElement data, string propertyName)
        {
            return data.TryGetProperty(propertyName, out var propertyValue) ? propertyValue.GetString() : string.Empty;
        }
    }

}

