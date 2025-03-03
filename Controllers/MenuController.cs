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
    public class MenuController : Controller
    {
        private readonly ConnectionDB _connection;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<MenuController> _logger;

        public MenuController(ConnectionDB connection, IWebHostEnvironment webHostEnvironment, ILogger<MenuController> logger)
        {
            _connection = connection;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

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

        public IActionResult GetSearchRecord(string SearchText)
        {
            List<EmployeeViewModel> list;
            if(!string.IsNullOrEmpty(SearchText))
            {
                list = _connection.Employee
                                .Where(x => x.Name.Contains(SearchText) || (x.Department.DepartmentName.Contains(SearchText)) || (x.Address.Contains(SearchText)))
                                .Select(x => new EmployeeViewModel
                                {
                                    Name = x.Name,
                                    EmployeeID = x.EmployeeID,
                                    DepartmentName = x.Department.DepartmentName,
                                    Address = x.Address
                                }).ToList();
            }
            else
            {
                list = _connection.Employee
                .Where(e => e.isDeleted == false)
                .Select(x => new EmployeeViewModel
                {
                    Name = x.Name,
                    EmployeeID = x.EmployeeID,
                    DepartmentName = x.Department.DepartmentName,
                    Address = x.Address
                }).ToList();
            }

            return PartialView("SearchPartial", list);
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


