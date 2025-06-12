using System.Security.Policy;

namespace pdf1.Models
{
    public class PdfModel
    {
        public IFormFile PdfFile { get; set; }
        public string ExtractedText { get; set; }
        public string ApiResponse { get; set; }
        public Url PdfUrl { get; set; }

    }
}
