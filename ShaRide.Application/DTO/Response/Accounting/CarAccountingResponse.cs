using System.Collections.Generic;

namespace ShaRide.Application.DTO.Response.Accounting
{
    public class CarAccountingResponse
    {
        public string CarTitle { get; set; }
        public string RegisterNumber { get; set; }
        public string CarBanAsset { get; set; }
        public decimal SumIncome { get; set; }
        public decimal Profit { get; set; }
        public IList<CarAccountingDetailedResponse> Detailed { get; set; }
    }
}