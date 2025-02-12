using Microsoft.AspNetCore.Mvc;
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
            List<Employee> employeeList = _connection.Employee.ToList();

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

            return View(employeeVM);
        }
    }
}
