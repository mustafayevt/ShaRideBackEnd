using System.ComponentModel.DataAnnotations.Schema;

namespace ShaRide.Domain.Entities
{
    [Table(nameof(RestrictionRideComposition),Schema = "Ride")]
    public class RestrictionRideComposition
    {
        public int RideId { get; set; }
        public virtual Ride Ride { get; set; }

        public int RestrictionId { get; set; }
        public virtual Restriction Restriction { get; set; }
        public bool IsPossible { get; set; }
    }
}