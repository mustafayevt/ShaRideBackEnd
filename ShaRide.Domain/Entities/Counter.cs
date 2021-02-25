using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class Counter : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
    }
}