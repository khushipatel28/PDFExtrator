using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pdf1.Models
{
    public class Project
    {
        [Key]
        public int ProId { get; set; }
        public string ProjectName { get; set; }
        public string Designation { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; } 
        public string Description { get; set; }
        //public string? Industry { get; set; }
        //public string? Type { get; set; }
        //public string? Duration { get; set; }
        //public List<Module> Modules { get; set; }

        [ForeignKey("Id")]
        public int Id { get; set; }
        
        public ResourceDetail res { get; set; }
    }
}
