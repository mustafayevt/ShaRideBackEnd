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
        public List<UserPhone> Phones { get; set; }
        public byte[] Img { get; set; }
        public string ImgExtension { get; set; }
        public List<UserRoleComposition> UserRoleComposition { get; set; }
    }
}