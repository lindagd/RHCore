using Microsoft.EntityFrameworkCore;
using RHCore.Models;

namespace RHCore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<EmployeeHistory> EmployeeHistories { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            await Task.Run(() => Console.WriteLine("SaveChangesAsync called!"));
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified && e.Entity is Employee)
                .ToList();

            foreach (var entry in entries)
            {
                var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);
                if (databaseValues == null)
                    continue;

                foreach (var attr in entry.Properties)
                {
                    var propertyName = attr.Metadata.Name;
                    var previousValue = databaseValues[propertyName];
                    var currentValue = entry.CurrentValues[propertyName];
                    Console.WriteLine($"{previousValue} => {currentValue}");
                    
                    if (previousValue.ToString() != currentValue.ToString())
                    {
                        var history = new EmployeeHistory
                        {
                            EmployeeId = ((Employee)entry.Entity).Id,
                            Fieldname = propertyName,
                            OldValue = FormatValue(previousValue),
                            NewValue = FormatValue(currentValue),
                            ChangedAt = DateTime.UtcNow,
                            User = "Admin"
                        };

                        Console.WriteLine($"Adicionando ao histórico: {history.Fieldname}, {history.OldValue} => {history.NewValue}");

                        await EmployeeHistories.AddAsync(history, cancellationToken);
                    }
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
        private static string FormatValue(object value)
        {
            if (value == null) return "N/A";

            return value switch
            {
                DateTime dateTime => dateTime.ToString("d"),
                decimal decimalValue => decimalValue.ToString("C"),
                bool b => b ? "Ativo" : "Inativo",
                _ => value.ToString()
            };
        }
    }
}
