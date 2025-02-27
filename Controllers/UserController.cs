using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCTutorial.Models;

namespace MVCTutorial.Controllers
{
    public class UserController : Controller
    {
        private readonly ConnectionDB _connection;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<UserController> _logger;

        public UserController(ConnectionDB connection, IWebHostEnvironment webHostEnvironment, ILogger<UserController> logger)
        {
            _connection = connection;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
        public IActionResult EmployeeUser()
        {
            var Employee = _connection.Employee.Where(x => x.isDeleted == false)
                                                .Select(x => new EmployeeViewModel
                                                {
                                                    EmployeeID = x.EmployeeID,
                                                    Name = x.Name,
                                                    DepartmentName = x.Department.DepartmentName,
                                                    Address = x.Address,
                                                }).ToList();
            var SiteUser = _connection.SiteUser.Select(x => new SiteUserViewModel
            {
                UserID = x.UserID,
                UserName = x.UserName,
                EmailId = x.EmailId,
                Address = x.Address,
                Password = x.Password,
                RoleID = x.RoleID,
                RoleName = x.UserRole.RoleName
            }).ToList();

            var viewModel = new EmployeeUserViewModel
            {
                Employee = Employee,
                SiteUser = SiteUser
            };

            return View(viewModel);
        }

        public IActionResult ShowUser(int UserID)
        {
            List<SiteUserViewModel> listUser = _connection.SiteUser.Where(x => x.UserID == UserID)
                                                      .Select(x => new SiteUserViewModel
                                                      {
                                                          UserName = x.UserName,
                                                          EmailId = x.EmailId,
                                                          Address = x.Address,
                                                          RoleName = x.UserRole.RoleName
                                                      }).ToList();
            ViewBag.roleList = listUser;

            return PartialView("PartialUser1");
        }
        [HttpPost]
        public JsonResult DeleteUser(int UserID)
        {
            bool result = false;

            SiteUser user = _connection.SiteUser.SingleOrDefault(x => x.UserID == UserID);
            if (user != null)
            {
                _connection.Remove(user);
                _connection.SaveChanges();
                result = true;
            }
            return Json(result);
        }

        [HttpGet]
        public IActionResult AddEditUser(int UserID)
        {
            SiteUserViewModel userViewModel = new SiteUserViewModel();
            List<UserRole> roleList = _connection.UserRole.ToList();
            ViewBag.roleList = new SelectList(roleList, "RoleID", "RoleName");



            if (UserID > 0)
            {
                SiteUser user = _connection.SiteUser
                                            .Include(x => x.UserRole)
                                            .SingleOrDefault(x => x.UserID == UserID);
                userViewModel.UserID = user.UserID;
                userViewModel.UserName = user.UserName;
                userViewModel.EmailId = user.EmailId;
                userViewModel.Password = user.Password;
                userViewModel.Address = user.Address;
                userViewModel.RoleID = user.UserRole.RoleID;
            }

            return PartialView("PartialUser2", userViewModel);
        }

        [HttpPost]
        public IActionResult UpdateUser(SiteUserViewModel model)
        {
            try
            {
                List<UserRole> roleList = _connection.UserRole.ToList();
                ViewBag.roleList = new SelectList(roleList, "RoleID", "RoleName");

                if (model.UserID > 0)
                {
                    //update
                    SiteUser user = _connection.SiteUser.SingleOrDefault(x => x.UserID == model.UserID);
                    user.RoleID = model.RoleID;
                    user.UserName = model.UserName;
                    user.EmailId = model.EmailId;
                    user.Password = model.Password;
                    user.Address = model.Address;

                    _connection.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return PartialView("PartialUser2");
        }

        [HttpPost]
        public IActionResult InsertUser(SiteUserViewModel model)
        {
            try
            {
                List<UserRole> roleList = _connection.UserRole.ToList();
                ViewBag.roleList = new SelectList(roleList, "RoleID", "RoleName");
                SiteUser user = _connection.SiteUser
                    .SingleOrDefault(x => x.UserID == model.UserID);

                if (model.UserID == 0)
                {
                    //insert
                    SiteUser userSite = new SiteUser();
                    userSite.RoleID = model.RoleID;
                    userSite.UserName = model.UserName;
                    userSite.EmailId = model.EmailId;
                    userSite.Address = model.Address;
                    userSite.Password = model.Password;

                    _connection.SiteUser.Add(userSite);
                    _connection.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("PartialUser2");
        }
    }
}
