using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Domain.Entities
{
    [Table(name: "Driver", Schema = "Driver")]
    public class Driver : BaseEntity
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string PlantNumber { get; set; }
        public string CarModel { get; set; }

    }
}
