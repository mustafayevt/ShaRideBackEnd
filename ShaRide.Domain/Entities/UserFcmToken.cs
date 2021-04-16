using System;
using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class UserFcmToken : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public string DeviceId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is UserFcmToken))
            {
                return false;
            }

            return Equals((UserFcmToken)obj);
        }

        protected bool Equals(UserFcmToken other)
        {
            return UserId == other.UserId && Equals(User, other.User) && Token == other.Token;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserId, User, Token);
        }
    }
}