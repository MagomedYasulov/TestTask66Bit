using Microsoft.AspNetCore.Mvc;

namespace TestTask66Bit.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        [HttpGet("internships")]
        public IActionResult Internships()
        {
            return View();
        }

        [HttpGet("interns/browse")]
        public IActionResult Interns()
        {
            return View();
        }

        [HttpGet("interns/create")]
        public IActionResult CreateIntern()
        {
            return View();
        }
    }
}
