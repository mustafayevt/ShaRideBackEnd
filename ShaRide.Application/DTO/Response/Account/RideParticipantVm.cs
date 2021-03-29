using System;

namespace ShaRide.Application.DTO.Response.Account
{
    public class RideParticipantVm
    {
        public int UserId { get; set; }
        
        public string UserFullname { get; set; }

        public short UserRating { get; set; }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        protected bool Equals(RideParticipantVm other)
        {
            return UserId == other.UserId && UserFullname == other.UserFullname && UserRating == other.UserRating;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserId, UserFullname, UserRating);
        }
    }
}