using System.ComponentModel.DataAnnotations.Schema;
using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class CarModel : BaseEntity
    {
        public string Title { get; set; }

        public int CarBrandId { get; set; }
        public virtual CarBrand CarBrand { get; set; }
    }
}