using Microsoft.AspNetCore.Mvc;

namespace pdf1.Controllers
{
    public class SkillController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
