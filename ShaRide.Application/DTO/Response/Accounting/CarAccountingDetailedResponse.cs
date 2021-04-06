using System;

namespace ShaRide.Application.DTO.Response.Accounting
{
    public class CarAccountingDetailedResponse
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal SumIncome { get; set; }
        public decimal Profit { get; set; }
        public decimal Commission { get; set; }
        public DateTime Date { get; set; }
    }
}