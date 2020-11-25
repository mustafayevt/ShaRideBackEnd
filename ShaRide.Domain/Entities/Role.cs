using System.ComponentModel.DataAnnotations;
using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class Role : BaseEntity
    {
        [Required]
        public string RoleName { get; set; }

        public Role(string roleName)
        {
            RoleName = roleName;
        }
    }
}