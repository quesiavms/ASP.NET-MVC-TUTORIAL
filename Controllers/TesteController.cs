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
        public IActionResult EmployeeDetail(int EmployeeID)
        {
            Employee employee = _connection.Employee.SingleOrDefault(x => x.EmployeeID == EmployeeID);
            EmployeeViewModel employeeVM = new EmployeeViewModel();

            employeeVM.Name = employee.Name;
            employeeVM.Address = employee.Address;
            employeeVM.DepartmentName = employee.Department.DepartmentName;
            employeeVM.isDeleted = employee.isDeleted;

            return View(employeeVM);
        }

        public IActionResult TesteRazor()
        {
            return View();
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

    }
}
