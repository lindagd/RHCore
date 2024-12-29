using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RHCore.Data;
using RHCore.Helpers;
using RHCore.Models;
using RHCore.ViewModels;

namespace RHCore.Controllers
{
    public class EmployeesController(ApplicationDbContext _context) : Controller
    {
        private readonly ApplicationDbContext _context = _context;

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var employee = await _context.Employees
                .Include(e => e.Vacation)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) return NotFound();

            EmployeeViewModel employeeViewModel = new EmployeeViewModel
                {
                    EmployeeName = employee.Name,
                    Role = employee.Role,
                    AdmissionDate = employee.AdmissionDate.ToString("d"),
                    Paycheck = employee.Paycheck.ToString("C"),
                    Status = ((employee.IsActive) ? "Ativo" : "Inativo"),
                    VacationStart = employee.Vacation != null ? employee.Vacation.StartingDate.ToString("d") : "-",
                    VacationEnd = employee.Vacation != null ? employee.Vacation.EndingDate.ToString("d") : "-",
                    VacationStatus = employee.Vacation == null ? "-" : VacationStatusHelper.Translate(employee.Vacation.VacationStatus)
                };

            return View(employeeViewModel);
        }

        [HttpGet]
        public IActionResult Create() { return View(); }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Role,AdmissionDate,Paycheck,IsActive")] Employee employee)
        {
            if (id != employee.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Employees.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
            }
            return View(employee);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult History()
        {
            var histories = _context.EmployeeHistories
                .Select(static e => new EmployeeHistoryViewModel
                {
                    EmployeeName = e.Employee.Name,
                    ChangedField = e.Fieldname,
                    OldValue = e.OldValue,
                    NewValue = e.NewValue,
                    ChangedAt = TimeZoneInfo.ConvertTimeFromUtc(e.ChangedAt, TimeZoneInfo.Local).ToString("g"),
                    ChangedBy = e.User
                }).ToList();
           
            return View(histories);
        }
    }
}
