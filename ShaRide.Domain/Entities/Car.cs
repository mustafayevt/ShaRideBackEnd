using System.Collections.Generic;
using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class Car : BaseEntity
    {
        public int CarModelId { get; set; }
        public CarModel CarModel { get; set; }
        public ICollection<CarImage> CarImages { get; set; }
        public string RegisterNumber { get; set; }
        public ICollection<CarSeatComposition> CarSeatComposition { get; set; }
    }
}