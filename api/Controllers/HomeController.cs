using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Spa()
        {
            return File("~/index.html", "text/html");
        }
    }
}
