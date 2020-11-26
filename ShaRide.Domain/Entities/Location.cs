using System.Collections.Generic;
using ShaRide.Domain.Common;
using ShaRide.Domain.Enums;

namespace ShaRide.Domain.Entities
{
    public class Location : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<LocationPoint> LocationPoints { get;}
    }
}