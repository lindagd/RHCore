namespace RHCore.Models
{
    public class EmployeeHistory
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public required string Fieldname { get; set; }
        public required string OldValue {  get; set; }
        public required string NewValue { get; set; }
        public DateTime ChangedAt { get; set; }
        public required string User { get; set; }

        public Employee? Employee { get; set; }
    }
}
