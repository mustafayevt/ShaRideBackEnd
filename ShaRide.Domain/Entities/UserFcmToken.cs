using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class UserFcmToken : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
    }
}