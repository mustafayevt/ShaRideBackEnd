using ShaRide.Application.DTO.Response.Car;
using ShaRide.Application.DTO.Response.Phone;
using ShaRide.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShaRide.Application.DTO.Response.Account
{
    public class UserFilterResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public ICollection<UserPhoneResponse> Phones { get; set; }
        public ICollection<string> Roles { get; set; }
        public ICollection<CarResponse> Cars { get; set; }
        public decimal Balance { get; set; }
        public short Rating { get; set; }

        public UserFilterResponse(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Surname = user.Surname;
            CreatedTimestamp = user.CreatedTimestamp;
            Roles = user.UserRoleComposition.Select(r => r.Role.RoleName).ToList();
            Balance = user.Balance;
        }
    }
}
