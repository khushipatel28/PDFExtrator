using System.ComponentModel.DataAnnotations;

namespace pdf1.Models
{
    public class TrainingAndCertificate
    {
        [Key]
        public int Id { get; set; }
        public List<Articleship> Articleships { get; set; }
        public List<Certificate> Certificates { get; set; }
    }
}
