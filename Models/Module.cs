using System.ComponentModel.DataAnnotations;

namespace pdf1.Models
{
    public class Module
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
