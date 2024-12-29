using Microsoft.EntityFrameworkCore;
using RHCore.Data.Enum;
using RHCore.Models;

namespace RHCore.Data
{
    public class Seed
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Employees.Any())
                {
                    return;
                }
                context.Employees.AddRange(
                    new Employee
                    {
                        Name = "Norma Blum",
                        Role = "Gestora de Inovação",
                        AdmissionDate = DateTime.Parse("2024-11-25"),
                        Paycheck = 3200,
                        IsActive = true
                    },
                    new Employee
                    {
                        Name = "Juliana Montes",
                        Role = "Desenvolvedor Pleno",
                        AdmissionDate = DateTime.Parse("2021-10-24"),
                        Paycheck = 3200,
                        IsActive = true
                    }
                );
                context.Vacations.AddRange(
                    new Vacation
                    {
                        StartingDate = DateTime.Parse("2025-02-03"),
                        EndingDate = DateTime.Parse("2025-02-14"),
                        VacationStatus = VacationStatus.Pending,
                        EmployeeId = 3,
                        Employee = new Employee
                        {
                            Name = "Michael Kyle",
                            Role = "Técnico de Redes",
                            AdmissionDate = DateTime.Parse("2023-05-20"),
                            Paycheck = 3200,
                            IsActive = true
                        }
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
