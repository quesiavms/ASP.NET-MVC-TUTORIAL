using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using MVCTutorial.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MVCTutorial.Controllers
{
    public class TesteController : Controller
    {
        private readonly ConnectionDB _connection;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<TesteController> _logger;

        public TesteController(ConnectionDB connection, IWebHostEnvironment webHostEnvironment, ILogger<TesteController> logger)
        {
            _connection = connection;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
        // interface user
        public IActionResult Index()
        {
            List<Employee> employeeList = _connection.Employee
                                          .Where(x => x.isDeleted == false)
                                          .ToList();

            List<EmployeeViewModel> employeeVMList = employeeList.Select(x => new EmployeeViewModel
            {
                Name = x.Name,
                EmployeeID = x.EmployeeID
            }).ToList();

            return View(employeeVMList);

        }

        public IActionResult ShowEmployee(int employeeID)
        {
            List<EmployeeViewModel> listEmp = _connection.Employee.Where(x => x.isDeleted == false && x.EmployeeID == employeeID)
                                                      .Select(x => new EmployeeViewModel
                                                      {
                                                          Name = x.Name,
                                                          DepartmentName = x.Department.DepartmentName,
                                                          Address = x.Address,
                                                          EmployeeID = x.EmployeeID,
                                                          SiteName = x.Sites.SiteName
                                                      }).ToList();
            ViewBag.EmployeeList = listEmp;

            return PartialView("Partial1");
        }

        public IActionResult EmployeeDetail(int EmployeeID)
        {
            Employee employee = _connection.Employee.SingleOrDefault(x => x.EmployeeID == EmployeeID);
            EmployeeViewModel employeeVM = new EmployeeViewModel();

            employeeVM.Name = employee.Name;
            employeeVM.Address = employee.Address;
            employeeVM.DepartmentName = employee.Department.DepartmentName;
            employeeVM.SiteName = employee.Sites.SiteName;

            return View(employeeVM);
        }
        // interface admin
        public IActionResult ManageEmployee()
        {
            List<EmployeeViewModel> listEmp = _connection.Employee.Where(x => x.isDeleted == false)
                                                                  .Select(x => new EmployeeViewModel
                                                                  {
                                                                      Name = x.Name,
                                                                      DepartmentName = x.Department.DepartmentName,
                                                                      Address = x.Address,
                                                                      EmployeeID = x.EmployeeID
                                                                  }).ToList();
            ViewBag.EmployeeList = listEmp;
            return View();
        }

        [HttpPost]
        public JsonResult DeleteEmployee(int EmployeeID)
        {
            bool result = false;

            Employee emp = _connection.Employee.SingleOrDefault(x => x.isDeleted == false && x.EmployeeID == EmployeeID);
            if (emp != null)
            {
                emp.isDeleted = true;
                _connection.SaveChanges();
                result = true;
            }
            return Json(result);
        }

        [HttpGet]
        public IActionResult AddEditEmployee(int employeeID)
        {
            EmployeeViewModel empViewModel = new EmployeeViewModel();
            List<Department> departmentList = _connection.Department.ToList();
            ViewBag.DepartmentList = new SelectList(departmentList, "DepartmentID", "DepartmentName");

            if (employeeID > 0)
            {
                Employee emp = _connection.Employee.SingleOrDefault(x => x.EmployeeID == employeeID && x.isDeleted == false);
                empViewModel.EmployeeID = emp.EmployeeID;
                empViewModel.DepartmentID = emp.DepartmentID;
                empViewModel.Name = emp.Name;
                empViewModel.Address = emp.Address;
                empViewModel.SiteName = emp.Sites.SiteName;
            }

            return PartialView("Partial2", empViewModel);
        }

        [HttpPost]
        public IActionResult UpdateEmployee(EmployeeViewModel model)
        {
            try
            {
                List<Department> departmentList = _connection.Department.ToList();
                ViewBag.DepartmentList = new SelectList(departmentList, "DepartmentID", "DepartmentName");

                if (model.EmployeeID > 0)
                {
                    //update
                    Employee emp = _connection.Employee.SingleOrDefault(x => x.EmployeeID == model.EmployeeID && x.isDeleted == false);
                    emp.DepartmentID = model.DepartmentID;
                    emp.Name = model.Name;
                    emp.Address = model.Address;
                    emp.Sites.SiteName = model.SiteName;

                    _connection.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return PartialView("Partial2");
        }

        [HttpPost]
        public IActionResult InsertEmployee(EmployeeViewModel model)
        {
            try
            {
                List<Department> departmentList = _connection.Department.ToList();
                ViewBag.DepartmentList = new SelectList(departmentList, "DepartmentID", "DepartmentName");
                Employee emp = _connection.Employee
                    .SingleOrDefault(x => x.EmployeeID == model.EmployeeID && x.isDeleted == false);

                if (model.EmployeeID == 0)
                {
                    //insert
                    Employee employee = new Employee();
                    employee.Name = model.Name;
                    employee.Address = model.Address;
                    employee.DepartmentID = model.DepartmentID;
                    employee.isDeleted = false;

                    _connection.Employee.Add(employee);
                    _connection.SaveChanges();

                    int latestEmpID = employee.EmployeeID;

                    Sites site = new Sites();
                    site.SiteName = model.SiteName;
                    site.EmployeeID = latestEmpID;

                    _connection.Sites.Add(site);
                    _connection.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("Partial2");
        }
        // new site user
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
        // Menu
        public IActionResult Menu()
        {
            List<EmployeeViewModel> list = _connection.Employee
                                            .Where(e => e.isDeleted == false)
                                            .Select(x => new EmployeeViewModel
                                        {
                                            Name = x.Name,
                                            EmployeeID = x.EmployeeID,
                                            DepartmentName = x.Department.DepartmentName,
                                            Address = x.Address
                                        }).ToList();

            ViewBag.EmployeeList = list;

            return View();
        }
        // search record
        public IActionResult GetSearchRecord(string SearchText)
        {
            List<EmployeeViewModel> list = _connection.Employee
                                            .Where(x => x.Name.Contains(SearchText) || (x.Department.DepartmentName.Contains(SearchText)))
                                            .Select(x => new EmployeeViewModel
                                        {
                                            Name = x.Name,
                                            EmployeeID = x.EmployeeID,
                                            DepartmentName = x.Department.DepartmentName,
                                            Address = x.Address
                                        }).ToList();


            return PartialView("SearchPartial", list);
        }
        // logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        // interface superadmin
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
        // anexar imagens
        [HttpPost]
        [RequestSizeLimit(104857600)]
        public async Task<IActionResult> UploadImage([FromForm] ProductViewModel model)
        {
            try
            {
                var file = model.ImageFile;
                if (file == null)
                    return BadRequest("Nenhum arquivo foi enviado.");

                byte[] imageByte;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    imageByte = memoryStream.ToArray();
                }

                var img = new ImageStore
                {
                    ImageName = file.FileName,
                    ImageByte = imageByte,
                    ImagePath = Path.Combine("UploadedImage", file.FileName),
                    IsDeleted = false
                };

                _connection.ImageStore.Add(img);
                 _connection.SaveChanges();

                return Json(new { imgID = img.ImageID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao fazer upload da imagem");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        [HttpGet]
        public IActionResult ImageRetrieve(int imgID)
        {
            var img = _connection.ImageStore.FirstOrDefault(x => x.ImageID == imgID);
            if (img == null || img.ImageByte == null)
                return NotFound();

            return File(img.ImageByte, GetContentType(img.ImageName));
        }
        private string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (provider.TryGetContentType(fileName, out string contentType))
            {
                return contentType;
            }
            return "application/octet-stream";
        }
        // dropdown
        public IActionResult Dropdown()
        {
            ViewBag.CountryList = new SelectList(GetCountryList(), "CountryID", "CountryName");

            return View();
        }
        public List<Country> GetCountryList()
        {
            List<Country> countries = _connection.Country.ToList();

            return countries;
        }

        [HttpGet]
        public IActionResult GetStateList(int countryID)
        {
            List<State> stateList = _connection.State.Where(x => x.CountryID == countryID).ToList();

            ViewBag.StateOptions = new SelectList(stateList, "StateID", "StateName");

            return PartialView("StateOptionsPartial");
        }



    }
}


