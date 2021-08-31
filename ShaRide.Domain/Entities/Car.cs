using System.Collections.Generic;
using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class Car : BaseEntity
    {
        public int CarModelId { get; set; }
        public CarModel CarModel { get; set; }
        public int BanTypeId { get; set; }
        public virtual BanType BanType { get; set; }
        public ICollection<CarImage> CarImages { get; set; }
        public string RegisterNumber { get; set; }
        public ICollection<CarSeatComposition> CarSeatComposition { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public bool IsDefault { get; set; }
    }
}