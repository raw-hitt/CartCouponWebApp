using Microsoft.AspNetCore.Mvc;

namespace CartWebApp.Controllers
{
    public class CartControllerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
