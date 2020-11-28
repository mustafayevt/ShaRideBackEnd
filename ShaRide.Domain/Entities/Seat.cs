using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class Seat : BaseEntity
    {
        public short xCordinant { get; set; }
        public short yCordinant { get; set; }
    }
}