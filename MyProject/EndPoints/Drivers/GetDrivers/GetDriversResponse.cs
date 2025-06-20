namespace MyProject.EndPoints.Drivers.GetDrivers
{
    public class GetDriversResponse
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string PlantNumber { get; set; }
        public string CarModel { get; set; }
        public Guid ID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
