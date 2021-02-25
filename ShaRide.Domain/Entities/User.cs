using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShaRide.Domain.Common;

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
        public short Rating { get; set; }
        public ICollection<UserPhone> Phones { get; set; }
        public ICollection<UserRoleComposition> UserRoleComposition { get; set; }
        public ICollection<UserImage> UserImages { get; set; }
    }
}