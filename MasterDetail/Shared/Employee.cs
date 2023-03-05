namespace MasterDetail.Shared
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = default!;
        public DateTime BirthDate { get; set; }
        public int Phone { get; set; }
        public string? Picture { get; set; } = null!;
        public bool MaritialStatus { get; set; }
        //Nev
        public virtual ICollection<BookingEntry>? BookingEntries { get; set; } = new List<BookingEntry>();
    }
}
