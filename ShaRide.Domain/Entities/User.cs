using ShaRide.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShaRide.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string PasswordHash { get; set; }

        public string UserUniqueKey { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public ICollection<UserPhone> Phones { get; set; }
        public ICollection<UserRoleComposition> UserRoleComposition { get; set; }
        public ICollection<UserImage> UserImages { get; set; }

        public decimal Balance { get; set; }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        protected bool Equals(User other)
        {
            return Name == other.Name && Surname == other.Surname && PasswordHash == other.PasswordHash && UserUniqueKey == other.UserUniqueKey && Equals(Phones, other.Phones) && Equals(UserRoleComposition, other.UserRoleComposition) && Equals(UserImages, other.UserImages) && Balance == other.Balance;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Surname, PasswordHash, UserUniqueKey, Phones, UserRoleComposition, UserImages, Balance);
        }
    }
}