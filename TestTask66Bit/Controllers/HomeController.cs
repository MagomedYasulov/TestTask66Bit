using Microsoft.AspNetCore.Mvc;

namespace TestTask66Bit.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
