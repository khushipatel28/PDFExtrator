using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pdf1.Models
{
    public class Skill
    {
        [Key]
        public int SId { get; set; }
        public string? Skills { get; set; }


        [ForeignKey("SkillgroupId")]
        public int SkillgroupId { get; set; }
        public Skillgroup skillgroup { get; set; }
        public ICollection<Resourceskill> rski { get; set; } = new List<Resourceskill>();
    }
}
