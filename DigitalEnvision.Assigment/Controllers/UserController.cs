using Microsoft.AspNetCore.Mvc;

namespace DigitalEnvision.Assigment.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
