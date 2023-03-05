using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MasterDetail.Shared
{
    public class EmployeeVM
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = default!;
        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public int Phone { get; set; }
        public string? Picture { get; set; }
        public IFormFile? PictureFile { get; set; }
        public bool MaritialStatus { get; set; }
        public List<Department> DepartmentList { get; set; } = new List<Department>();

        public virtual ICollection<BookingEntry>? BookingEntries { get; set; } = new List<BookingEntry>();

    }
}
