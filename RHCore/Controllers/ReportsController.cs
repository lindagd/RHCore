using Microsoft.AspNetCore.Mvc;
using RHCore.Data;
using RHCore.ViewModels;
using RHCore.Helpers;
using QuestPDF.Fluent;
using RHCore.Services;

namespace RHCore.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult GenerateReport()
        {
            return View();
        }

        public IActionResult GeneratePdf()
        {
            var reportData = _context.Employees
                .Select(static e => new EmployeeViewModel
                {
                    EmployeeName = e.Name,
                    Role = e.Role,
                    AdmissionDate = e.AdmissionDate.ToString("d"),
                    Paycheck = e.Paycheck.ToString("C"),
                    Status = ((e.IsActive) ? "Ativo" : "Inativo"),
                    VacationStart = e.Vacation != null ? e.Vacation.StartingDate.ToString("d") : "-",
                    VacationEnd = e.Vacation != null ? e.Vacation.StartingDate.ToString("d") : "-",
                    VacationStatus = e.Vacation == null ? "-" : VacationStatusHelper.Translate(e.Vacation.VacationStatus)
                }).ToList();

            var PdfDoc = Document.Create(document =>
            {
                var generator = new EmployeesReportPdfGenerator(reportData);
                generator.Generate(document);
            });

            var pdf = PdfDoc.GeneratePdf();
            return File(pdf, "application/pdf", "Relatorio_Funcionarios.pdf");
        }
    }
}
