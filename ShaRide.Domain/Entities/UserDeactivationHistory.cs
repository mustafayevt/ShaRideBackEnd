using System;

namespace ShaRide.Domain.Entities
{
    public class UserDeactivationHistory
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public DateTime DeactivationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}