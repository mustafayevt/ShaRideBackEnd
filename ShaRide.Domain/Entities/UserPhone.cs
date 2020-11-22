using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class UserPhone : BaseEntity
    {
        public string Number { get; set; }
        public bool IsMain { get; set; }
        public bool IsConfirmed { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}