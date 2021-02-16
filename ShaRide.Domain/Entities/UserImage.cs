using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class UserImage : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public byte[] Image { get; set; }
        public string Extension { get; set; }
    }
}