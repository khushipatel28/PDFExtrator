using System.ComponentModel.DataAnnotations;

namespace pdf1.Models
{
    public class ProfileSummary
    {
        [Key]
        public int Id { get; set; }
        public List<Skill> Skills { get; set; }
    }
}
