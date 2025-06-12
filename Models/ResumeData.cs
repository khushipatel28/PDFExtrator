using System.ComponentModel.DataAnnotations;

namespace pdf1.Models
{
    public class ResumeData
    {
        public PersonalInformation PersonalInformation { get; set; }
        
        public string Objective { get; set; }
        public ProfileSummary ProfileSummary { get; set; }
        public List<Experience> Experience { get; set; }
        public ERPProficiency ERPProficiency { get; set; }
        public List<string> ComputerProficiency { get; set; }
        public List<string> Languages { get; set; }
        public List<ProjectSupport> ProjectsSupport { get; set; }
        public List<string> Achievements { get; set; }
        public List<Education> Education { get; set; }

        //public Dictionary<string, Education> Education { get; set; }
        public TrainingAndCertificate TrainingAndCertificates { get; set; }
    }
}
