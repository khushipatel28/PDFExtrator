using System.ComponentModel.DataAnnotations;

namespace pdf1.Models
{
    public class ERPProficiency
    {
        [Key]
        public int Id { get; set; }
        public List<MicrosoftProduct> MicrosoftProducts { get; set; }
        public List<Module> Modules { get; set; }
    }
}
