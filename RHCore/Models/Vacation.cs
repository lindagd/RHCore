using System.ComponentModel.DataAnnotations.Schema;
using RHCore.Data.Enum;

namespace RHCore.Models
{
    public class Vacation
    {
        public int Id { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public VacationStatus VacationStatus { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
