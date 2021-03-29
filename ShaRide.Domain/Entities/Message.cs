using ShaRide.Domain.Common;
using ShaRide.Domain.Enums;

namespace ShaRide.Domain.Entities
{
    public class Message : AuditableBaseEntity
    {
        public string Content { get; set; }
        
        public MessageType MessageType { get; set; }
        
        public MessageSenderType SenderType { get; set; }
        
        public int RideId { get; set; }
        
        public virtual Ride Ride { get; set; }
    }
}