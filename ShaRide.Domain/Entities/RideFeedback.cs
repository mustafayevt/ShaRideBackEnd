using System.Collections.Generic;

namespace ShaRide.Domain.Entities
{
    public class RideFeedback : Message
    {
        public ICollection<Attachment> Files { get; set; }
    }
}