using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pdf1.Models
{
    public class Certificate
    {
        [Key]
        public int CerId { get; set; }
        public string? Name { get; set; }
        public string? Provider { get; set; }
        public string? Date { get; set; }
        public string? CertificateUrl { get; set; }
        public string? Description { get; set; }

        [ForeignKey("Id")]
        public int Id { get; set; }
        
        public ResourceDetail res { get; set; }
    }
}
