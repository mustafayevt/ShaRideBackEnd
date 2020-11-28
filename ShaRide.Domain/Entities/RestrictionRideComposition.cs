namespace ShaRide.Domain.Entities
{
    public class RestrictionRideComposition
    {
        public int RideId { get; set; }
        public virtual Ride Ride { get; set; }

        public int RestrictionId { get; set; }
        public virtual Restriction Restriction { get; set; }
        public bool IsPossible { get; set; }
    }
}