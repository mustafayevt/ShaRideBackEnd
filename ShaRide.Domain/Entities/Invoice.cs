using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public virtual User User { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        
        public decimal Amount { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }

        public DateTime InvoiceRegisterDate { get; set; }
    }
}