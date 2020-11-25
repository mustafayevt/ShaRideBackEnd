using System.ComponentModel.DataAnnotations;
using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class UserPhone : BaseEntity
    {
        [Required]
        public string Number { get; set; }
        public bool IsMain { get; set; }
        public bool IsConfirmed { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}