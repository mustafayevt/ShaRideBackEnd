using System;

namespace ShaRide.Application.DTO.Response.Accounting
{
    public class PaymentDetailResponse
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string UserUniqueKey { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string PaymentProvider { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
    }
}