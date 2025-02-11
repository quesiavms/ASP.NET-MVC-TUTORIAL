using Microsoft.AspNetCore.Mvc;
using MVCTutorial.Models;

namespace MVCTutorial.Controllers
{
    public class TesteController : Controller
    {
        public IActionResult IndexTeste()
        {
            ViewBag.Name = "Qué";
            List<string> myList = new List<string> 
            { "Quesia", 
              "Queren",
              "Paulo",
              "Marcia"
            };
            ViewBag.NameList = myList;

            List<Employee> employeeList = new List<Employee>
            {
                new Employee { EmployeeID = 1, Name = "Quesia", Department = "IT" },
                new Employee { EmployeeID = 2, Name = "Queren", Department = "QA" },
                new Employee { EmployeeID = 3, Name = "Paulo", Department = "Inventory" },
                new Employee { EmployeeID = 4, Name = "Marcia", Department = "Sales" }
            };
            //ViewBag.EmployeeList = employeeList;
            ViewData["EmployeeList"] = employeeList;

            ViewBag.EmployeeNameVB = "Maria";

            ViewData["EmployeeNameVD"] = "Joana";

            TempData["EmployeeNameTD"] = "Ana";

            TempData.Keep();

            return View(employeeList);
        }

        public IActionResult SecondPage()
        {
            return View();
        }
    }
}
