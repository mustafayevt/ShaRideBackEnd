using ShaRide.Domain.Common;
using ShaRide.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShaRide.Domain.Entities
{
    public class RideFeedback : AuditableBaseEntity
    {
        public string Content { get; set; }

        public MessageType MessageType { get; set; }

        public MessageSenderType SenderType { get; set; }

        public int RideId { get; set; }

        public virtual Ride Ride { get; set; }
        public ICollection<Attachment> Files { get; set; }
    }
}