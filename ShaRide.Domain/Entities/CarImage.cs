using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class CarImage : BaseEntity
    {
        public int CarId { get; set; }
        public Car Car { get; set; }

        public byte[] Image { get; set; }
        public string Extension { get; set; }
    }
}