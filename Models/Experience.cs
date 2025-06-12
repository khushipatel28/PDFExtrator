using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pdf1.Models
{
    public class Experience
    {
        [Key]
        public int ExpId { get; set; }
        public string? CompanyName { get; set; }
       // public string? Organization { get; set; }
       // public string? Duration { get; set; }
        public string? Designation { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Description { get; set; }

        [ForeignKey("Id")]
        public int Id { get; set; }
        
        public ResourceDetail res { get; set; }
    }
}
