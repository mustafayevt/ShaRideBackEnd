using System.ComponentModel.DataAnnotations.Schema;
using ShaRide.Domain.Enums;

namespace ShaRide.Domain.Entities
{
    [Table(nameof(RideLocationPointComposition),Schema = "Ride")]
    public class RideLocationPointComposition
    {
        public int RideId { get; set; }
        public virtual Ride Ride { get; set; }

        public int LocationPointId { get; set; }
        public LocationPoint LocationPoint { get; set; }
        public LocationPointType LocationPointType { get; set; }

    }
}