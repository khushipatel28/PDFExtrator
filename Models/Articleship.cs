using System.ComponentModel.DataAnnotations;

namespace pdf1.Models
{
    public class Articleship
    {
        [Key]
        public int Id { get; set; }
        public string? Firm { get; set; }
        public string? Duration { get; set; }
        public List<Responsibility> Responsibilities { get; set; }
    }
}
