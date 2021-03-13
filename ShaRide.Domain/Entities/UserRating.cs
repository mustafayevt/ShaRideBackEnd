using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class UserRating : BaseEntity
    {
        public int SourceUserId { get; set; }
        public User SourceUser { get; set; }
        public int DestinationUserId { get; set; }
        public User DestinationUser { get; set; }
        public short Value { get; set; }
        public Ride Ride { get; set; }
        [ForeignKey(nameof(Ride))]
        public int RideId { get; set; }
    }
}