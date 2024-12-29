using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RHCore.Data;
using RHCore.Data.Enum;
using RHCore.Models;

namespace RHCore.Controllers
{
    public class VacationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VacationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Vacation> vacations = _context.Vacations.Include(v => v.Employee).ToList();

            return View(vacations);
        }

        public IActionResult Details(int id)
        {
            return View();
        }

        public IActionResult Create()
        {
            SetupViewBag();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Vacation vacation)
        {
            if (ModelState.IsValid)
            {
                _context.Vacations.Add(vacation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vacation);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacation = await _context.Vacations.FindAsync(id);
            if (vacation == null)
            {
                return NotFound();
            }

            SetupViewBag();

            return View(vacation);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartingDate,EndingDate,VacationStatus,EmployeeId")] Vacation vacation)
        {
            if (id != vacation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction($"{nameof(Index)}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Vacations.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(vacation);
        }
        private void SetupViewBag()
        {
            ViewBag.Employees = _context.Employees
            .Where(e => e.IsActive)
            .Select(e => new { e.Id, e.Name })
            .ToList();

            ViewBag.VacationStatus = new List<SelectListItem>
            {
                new() { Value = VacationStatus.Pending.ToString(), Text = "Pendente" },
                new() { Value = VacationStatus.Ongoing.ToString(), Text = "Em Andamento" },
                new() { Value = VacationStatus.Completed.ToString(), Text = "Concluída" }
            };
        }
    }
}
