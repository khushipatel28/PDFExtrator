using System.ComponentModel.DataAnnotations;

namespace pdf1.Models
{
    public class CreateSkillgroupViewModel
    {
        [Required]
        public string Skill_group { get; set; }
    }
}
