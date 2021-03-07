using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class Feedback : AuditableBaseEntity
    {
        public string Content { get; set; }

        public Feedback()
        {
            
        }

        public Feedback(string content)
        {
            Content = content;
        }
    }
}