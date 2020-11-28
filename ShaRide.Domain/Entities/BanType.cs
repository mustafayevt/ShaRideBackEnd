using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class BanType : BaseEntity
    {
        public string Title { get; set; }
        public string AssetPath { get; set; }
    }
}