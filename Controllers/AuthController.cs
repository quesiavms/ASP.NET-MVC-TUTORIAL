using Microsoft.AspNetCore.Mvc;
using MVCTutorial.Models;

namespace MVCTutorial.Controllers
{
    public class AuthController : Controller
    {
        private readonly ConnectionDB _connection;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ConnectionDB connection, IWebHostEnvironment webHostEnvironment, ILogger<AuthController> logger)
        {
            _connection = connection;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(RegistrationViewModel model)
        {
            SiteUser siteUser = new SiteUser();
            siteUser.UserName = model.UserName;
            siteUser.EmailId = model.EmailId;
            siteUser.Password = model.Password;
            siteUser.Address = model.Address;
            siteUser.RoleID = 3;

            _connection.SiteUser.Add(siteUser);
            _connection.SaveChanges();

            return Json(new { success = true, message = "Register done with success!" });
        }
        // login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoginUser(LoginViewModel model)
        {
            SiteUser user = _connection.SiteUser.SingleOrDefault(x => x.EmailId == model.EmailId && x.Password == model.Password);
            string result = "Fail";
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserID", user.UserID);
                HttpContext.Session.SetString("UserName", user.UserName);

                if (user.RoleID == 3)
                {
                    result = "General User";
                }
                else if (user.RoleID == 1)
                {
                    result = "Admin";
                }
                else if (user.RoleID == 2)
                {
                    result = "SuperAdmin";
                }
            }
            return Json(result);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
