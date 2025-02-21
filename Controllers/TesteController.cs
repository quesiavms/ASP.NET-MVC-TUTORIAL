using System.Linq.Expressions;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Graph.Models;
using MVCTutorial.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MVCTutorial.Controllers
{
    public class TesteController : Controller
    {
        private readonly ConnectionDB _connection;

        public TesteController(ConnectionDB connection)
        {
            _connection = connection;
        }
        public IActionResult Index()
        {
            List<Employee> employeeList = _connection.Employee
                                          .Where(x => x.isDeleted ==  false)
                                          .ToList();

            List<EmployeeViewModel> employeeVMList = employeeList.Select(x => new EmployeeViewModel
            {
                Name = x.Name,
                EmployeeID = x.EmployeeID
            }).ToList();

            return View(employeeVMList);

        }
        public IActionResult TesteRazor()
        {
            return View();
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


        public IActionResult AddInfo()
        {
            List<Department> departmentList = _connection.Department.ToList();
            ViewBag.DepartmentList = new SelectList(departmentList, "DepartmentID", "DepartmentName");

            return View();
        }

        //public IActionResult SaveRecord(EmployeeViewModel model)
        //{
        //    try
        //    {
        //        Employee employee = new Employee();
        //        employee.Name = model.Name;
        //        employee.Address = model.Address;
        //        employee.DepartmentID = model.DepartmentID;

        //        _connection.Employee.Add(employee); // adicionando employee no banco
        //        _connection.SaveChanges(); // salvando os dados

        //        int latestEmpID = employee.EmployeeID;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return RedirectToAction("AddInfo"); // retornando para a tela do AddInfo
        //}

        [HttpPost]
        public IActionResult AddInfo(EmployeeViewModel model)
        {
            try
            {
                List<Department> departmentList = _connection.Department.ToList();
                ViewBag.DepartmentList = new SelectList(departmentList, "DepartmentID", "DepartmentName");

                Employee employee = new Employee();
                employee.Name = model.Name;
                employee.Address = model.Address;
                employee.DepartmentID = model.DepartmentID;

                _connection.Employee.Add(employee); //adding employee no banco
                _connection.SaveChanges(); // saving

                int latestEmpID = employee.EmployeeID;

                Sites site = new Sites();
                site.SiteName = model.SiteName;
                site.EmployeeID = latestEmpID;

                _connection.Sites.Add(site);
                _connection.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("AddInfo");
        }

        public IActionResult DeleteEmployee()
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
            if(emp != null)
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

                if(model.EmployeeID > 0)
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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoginUser(LoginViewModel model)
        {
            SiteUser user = _connection.SiteUser.SingleOrDefault(x => x.EmailId == model.EmailId && x.Password == model.Password);
            string result = "Fail";
            if(user != null)
            {
                HttpContext.Session.SetInt32("UserID", user.UserID);
                HttpContext.Session.SetString("UserName", user.UserName);

                if (user.RoleID == 3)
                {
                    result = "General User";
                }
                else if(user.RoleID == 1)
                {
                    result = "Admin";
                } 
            }
            return Json(result);
        }

        public IActionResult Menu()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

