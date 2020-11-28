using System.ComponentModel.DataAnnotations.Schema;
using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class CarModel : BaseEntity
    {
        public string Title { get; set; }

        public int CarBrandId { get; set; }
        public virtual CarBrand CarBrand { get; set; }
        
        public int BanTypeId { get; set; }
        public virtual BanType BanType { get; set; }
    }
}