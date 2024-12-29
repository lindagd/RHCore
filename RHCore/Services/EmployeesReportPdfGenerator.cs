using QuestPDF.Helpers;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using RHCore.ViewModels;

namespace RHCore.Services
{
    public class EmployeesReportPdfGenerator
    {
        private readonly List<EmployeeViewModel> _employees;

        public EmployeesReportPdfGenerator(List<EmployeeViewModel> employees)
        {
            _employees = employees;
        }

        public void Generate(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Header()
                    .Text("RH.Core - Relatório de Funcionários").AlignCenter()
                    .SemiBold().FontSize(16).LineHeight(3);


                page.Content()
                    .PaddingBottom(16)
                    .Table(table =>
                    {
                        IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                        {
                            return container
                                .Border(1)
                                .BorderColor(Colors.Grey.Lighten1)
                                .Background(backgroundColor)
                                .PaddingVertical(3)
                                .PaddingHorizontal(10)
                                .AlignMiddle();
                        }

                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.ConstantColumn(80);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Nome").Bold();
                            header.Cell().Element(CellStyle).Text("Cargo").Bold();
                            header.Cell().Element(CellStyle).Text("Data de Admissão").Bold().AlignCenter();
                            header.Cell().Element(CellStyle).Text("Salário").Bold().AlignCenter();
                            header.Cell().Element(CellStyle).Text("Status").Bold().AlignCenter();

                            IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                        });

                        foreach (var e in _employees)
                        {
                            table.Cell().Element(CellStyle).Text(e.EmployeeName);
                            table.Cell().Element(CellStyle).Text(e.Role);
                            table.Cell().Element(CellStyle).Text(e.AdmissionDate).AlignCenter();
                            table.Cell().Element(CellStyle).Text(e.Paycheck).AlignCenter();
                            table.Cell().Element(CellStyle).Text(e.Status).AlignCenter();

                            IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.White).ShowOnce();
                        }
                    });
            });
        }
    }
}
