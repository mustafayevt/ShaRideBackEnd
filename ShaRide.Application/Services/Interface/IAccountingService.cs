using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Response.Accounting;

namespace ShaRide.Application.Services.Interface
{
    public interface IAccountingService
    {
        Task<CarAccountingGeneralInfoResponse> GetCarAccounting();
        Task<IList<PaymentDetailResponse>> GetPaymentDetails();
    }
}