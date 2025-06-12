using System.ComponentModel.DataAnnotations;

namespace pdf1.Models
{
    public class PersonalInformation
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? LinkedIn { get; set; }
    }
}
