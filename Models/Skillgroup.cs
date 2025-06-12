using System.ComponentModel.DataAnnotations;

namespace pdf1.Models
{
    public class Skillgroup
    {
        [Key]
        public int SkillgroupId { get; set; }

        public string Skill_group {  get; set; }

        public ICollection<Skill> Sk { get; set; } = new List<Skill>();
    }
}
