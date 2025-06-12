using System.ComponentModel.DataAnnotations;

namespace pdf1.Models
{
    public class ProjectSupport
    {
        [Key]
        public int Id { get; set; }
        public string? Organization { get; set; }
        public string? ProjectIndustry { get; set; }
        public string? ProjectType { get; set; }
        public List<Module> Modules { get; set; }
        public List<Project> Projects { get; set; }
        public string? Support { get; set; }
        public string? AppSourceISV { get; set; }
    }
}
