using System;
using System.ComponentModel.DataAnnotations.Schema;
using ShaRide.Domain.Entities;

namespace ShaRide.Domain.Common
{
    public abstract class AuditableBaseEntity : BaseEntity
    {
        [ForeignKey("ApplicationUser")]
        public int CreatedBy { get; set; }
        public virtual User User { get; set; }
        public DateTime Created { get; set; }
        [ForeignKey("ApplicationUser")]
        public int LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}