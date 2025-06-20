using System.ComponentModel.DataAnnotations;
using UUIDNext;

namespace MyProject.Domain.Entities
{
    public class BaseEntity
    {
        [Required]
        [MaxLength(36)]
        [Key]
        public Guid ID { get; set; } = Uuid.NewDatabaseFriendly(Database.SqlServer);
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        [MaxLength(36)]
        public Guid? CreatedBy { get; set; } 
        [MaxLength(36)]
        public Guid? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime TTL { get; set; } = DateTime.Now.AddMonths(3);
    }
}
