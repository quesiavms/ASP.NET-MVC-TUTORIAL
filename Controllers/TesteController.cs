using Microsoft.AspNetCore.Mvc;
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
            List<Employee> employeeList = _connection.Employee.ToList();

            List<EmployeeViewModel> employeeVMList = employeeList.Select(x => new EmployeeViewModel
            {
                Name = x.Name,
                EmployeeID = x.EmployeeID,
                DepartmentID = x.DepartmentID,
                Address = x.Address,
                DepartmentName = x.Department.DepartmentName
            }).ToList();

            return View(employeeVMList);

        }


        public IActionResult SecondPage()
        {
            return View();
        }
    }
}
