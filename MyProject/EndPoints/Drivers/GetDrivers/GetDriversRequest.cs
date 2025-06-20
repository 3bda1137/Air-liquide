using System.ComponentModel;

namespace MyProject.EndPoints.Drivers.GetDrivers
{
    public class GetDriversRequest
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PlantNumber { get; set; }
        public string? CarModel { get; set; }
        public DateOnly? FromDate { get; init; }
        public DateOnly? ToDate { get; init; }
        [DefaultValue("CreatedAt")]
        public string? OrderBy { get; init; }
        [DefaultValue(false)]
        public bool? IsAscending { get; init; }
        [DefaultValue(1)]
        public int? PageIndex { get; init; }
        [DefaultValue(100)]
        public int? PageSize { get; init; }
    }
}
