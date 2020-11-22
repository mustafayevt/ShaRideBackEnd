using System.Collections.Generic;
using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class ApplicationUser : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PasswordHash { get; set; }
        public List<UserPhone> Phones { get; set; }
        public byte[] Img { get; set; }
        public string ImgExtension { get; set; }
    }
}