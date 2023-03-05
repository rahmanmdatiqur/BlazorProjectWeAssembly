using System.ComponentModel.DataAnnotations.Schema;

namespace MasterDetail.Shared
{
    public class BookingEntry
    {
        public int BookingEntryId { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        //Navigation
        public virtual Employee? Employee { get; set; }
        public virtual Department?  Department { get; set; }
    }
}
