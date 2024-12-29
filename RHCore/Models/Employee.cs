using System.ComponentModel.DataAnnotations.Schema;

namespace RHCore.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public DateTime AdmissionDate { get; set; }
        public decimal Paycheck { get; set; }
        public bool IsActive { get; set; }
        public Vacation? Vacation { get; set; }
    }
}
