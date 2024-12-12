using Microsoft.AspNetCore.Mvc;

namespace api_lib.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
