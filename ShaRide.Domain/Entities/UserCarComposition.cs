using System.ComponentModel.DataAnnotations.Schema;
using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    [Table("CarUserComposition", Schema = "Ride")]
    public class UserCarComposition : BaseEntity
    {
        public int CarId { get; set; }
        public Car Car { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}