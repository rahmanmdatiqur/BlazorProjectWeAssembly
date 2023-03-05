using System.Text.Json.Serialization;

namespace MasterDetail.Shared
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; } = default!;
        //Nev
        [JsonIgnore]
        public virtual ICollection<BookingEntry>? BookingEntries { get; set; } = new List<BookingEntry>();
    }
}
