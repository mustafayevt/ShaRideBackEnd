using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.Invoice;
using ShaRide.Application.DTO.Response.Invoice;

namespace ShaRide.Application.Services.Interface
{
    public interface IInvoiceService
    {
        Task<InvoiceResponse> RegisterInvoice(RegisterInvoiceRequest request);
        Task<InvoiceResponse> GetInvoiceDetail(string invoiceNumber);
    }
}