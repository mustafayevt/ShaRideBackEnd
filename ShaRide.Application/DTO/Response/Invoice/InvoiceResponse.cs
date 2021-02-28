using System;

namespace ShaRide.Application.DTO.Response.Invoice
{
    public class InvoiceResponse
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceRegisterDate { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}