using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using ShaRide.Domain.Common;
using ShaRide.Domain.Enums;

namespace ShaRide.Domain.Entities
{
    public class LocationPoint : BaseEntity
    {
        [ForeignKey(nameof(Location))]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        public string Name { get; set; }
        public LocationPointType LocationPointType { get; set; }
    }
}