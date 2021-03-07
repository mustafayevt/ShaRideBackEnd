using System;
using System.ComponentModel.DataAnnotations.Schema;
using ShaRide.Domain.Entities;

namespace ShaRide.Domain.Common
{
    public abstract class AuditableBaseEntity : BaseEntity
    {
        [ForeignKey("ApplicationUser")]
        public int CreatedByUserId { get; set; }
        public virtual User CreatedByUser { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        [ForeignKey("ApplicationUser")]
        public int? LastModifiedByUserId { get; set; }
        public virtual User LastModifiedByUser { get; set; }
        public DateTime? LastModifiedTimestamp { get; set; }
    }
}