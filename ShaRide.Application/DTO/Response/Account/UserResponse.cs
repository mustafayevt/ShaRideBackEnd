using System.Collections.Generic;
using ShaRide.Application.DTO.Response.UserPhone;

namespace ShaRide.Application.DTO.Response.Account
{
    public class UserResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public short Rating { get; set; }
        public ICollection<UserPhoneResponse> Phones { get; set; }
    }
}