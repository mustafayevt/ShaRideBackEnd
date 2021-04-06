using System.Collections.Generic;

namespace ShaRide.Application.DTO.Response.Accounting
{
    public class CarAccountingGeneralInfoResponse
    {
        public decimal SumIncome { get; set; }
        public decimal Profit { get; set; }
        public decimal Commission => SumIncome - Profit;
        public IList<CarAccountingResponse> CarAccountings { get; set; }
    }
}