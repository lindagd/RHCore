using Microsoft.AspNetCore.Mvc;
using RHCore.Data;
using RHCore.Models;

namespace RHCore.Controllers
{
    public class HomeController(ApplicationDbContext context) : Controller
    {
        public IActionResult Index()
        {
            List<Employee> employees = [.. context.Employees];
            int activeEmployees = employees.Count(e => e.IsActive);
            decimal averagePaycheck = employees.Average(e => e.Paycheck);
            ViewBag.ActiveEmployeesCount = activeEmployees;
            ViewBag.AveragePaycheck = averagePaycheck;

            return View(employees);
        }
    }
}
