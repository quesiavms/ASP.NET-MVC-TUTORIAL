﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCTutorial.Models;

namespace MVCTutorial.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ConnectionDB _connection;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ConnectionDB connection, IWebHostEnvironment webHostEnvironment, ILogger<EmployeeController> logger)
        {
            _connection = connection;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
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
            Employee employee = _connection.Employee
                                .Include(e => e.Department)
                                .Include(e=> e.Sites)
                                .SingleOrDefault(x => x.EmployeeID == EmployeeID);
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
                Employee emp = _connection.Employee
                                .Include(e => e.Department)
                                .Include(e => e.Sites)
                                .SingleOrDefault(x => x.EmployeeID == employeeID && x.isDeleted == false);
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
    }
}
