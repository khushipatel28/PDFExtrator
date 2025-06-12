using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pdf1.Models
{
    public class Resourceskill
    {
        [Key]
        public int? ResourceskillId { get; set; }

        [ForeignKey("ResId")]
        public int? ResId { get; set; }
        public ResourceDetail res { get; set; }

        [ForeignKey("SkillId")]
        public int? SkillId { get; set; }
        public Skill sk { get; set; }
    }
}
