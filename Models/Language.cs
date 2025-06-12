using System.ComponentModel.DataAnnotations;

namespace pdf1.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
