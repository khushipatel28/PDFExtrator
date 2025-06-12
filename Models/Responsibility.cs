using System.ComponentModel.DataAnnotations;

namespace pdf1.Models
{
    public class Responsibility
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }
    }
}
