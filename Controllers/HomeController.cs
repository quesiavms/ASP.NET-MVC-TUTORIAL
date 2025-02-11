using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVCTutorial.Models;

namespace MVCTutorial.Controllers
    /*
    localhost60205/Home/Index
    server/controllerName/ActionMethod    
     */
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public string HelloWorld() // adicionar na url '/home/HelloWorld'
        {
            return "Hello world";
        }

        public string GetName(string name) // adicionar na url '/home/GetName?Name=Quesia'
        {
            return "Name = " + name;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
