using System.ComponentModel.DataAnnotations;

namespace pdf1.Models
{
    public class ResourceDetail
    {
        [Key]
        public int ResourceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string LinkedInUrl { get; set;}
        public string YearOfExperience { get; set; }
        public string AboutMe { get; set; }

        public ICollection<Education> edu { get; set; } = new List<Education>();
        public ICollection<Experience> exp { get; set; } = new List<Experience>();
        public ICollection<Project> pro { get; set; } = new List<Project>();
        public ICollection<Certificate> cer { get; set; } = new List<Certificate>();
        public ICollection<Skill> ski { get; set; } = new List<Skill>();
        public ICollection<Resourceskill> rski { get; set; } = new List<Resourceskill>();


    }
}
