using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pdf1.Models
{
    public class Education
    {
        [Key]
        public int EduId { get; set; }
       // public string? Degree { get; set; }
       // public string? Duration { get; set; }
       // public string? Institution { get; set; }
       // public string? Achievements { get; set; }

        public string UniversityName { get; set; }
        public string Degree { get; set; }
        public string Course { get; set; }
        public string StartMonthYear { get; set; }
        public string EndMonthYear { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }

        [ForeignKey("Id")]
        public int Id { get; set; }
       
        public ResourceDetail res { get; set; }
    }
}
