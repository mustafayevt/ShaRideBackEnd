using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Invoice;
using ShaRide.Application.DTO.Response.Invoice;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Services.Concrete
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public InvoiceService(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<InvoiceResponse> RegisterInvoice(RegisterInvoiceRequest request)
        {
            var user = await _applicationDbContext.Users.AsTracking()
                .FirstOrDefaultAsync(x => x.IsRowActive && x.UserUniqueKey == request.UserId);
            if (user is null)
                throw new ApiException($"User with id : '{request.UserId}' not found in our system");

            var invoiceExists = await _applicationDbContext.Invoices.AnyAsync(x=>x.IsRowActive && x.InvoiceNumber == request.InvoiceNumber);
            if (invoiceExists)
                throw new ApiException($"Invoice with number : '{request.InvoiceNumber}' already exist");

            var newInvoice = _mapper.Map<Invoice>(request);

            newInvoice.UserId = user.Id;

            user.Balance += newInvoice.Amount;

            await _applicationDbContext.Invoices.AddAsync(newInvoice);

            await _applicationDbContext.SaveChangesAsync();

            var response = _mapper.Map<InvoiceResponse>(request);
            response.Name = user.Name;
            response.Surname = user.Surname;
            return response;
        }

        public async Task<InvoiceResponse> GetInvoiceDetail(string invoiceNumber)
        {
            var invoice = await _applicationDbContext.Invoices
                .Include(x=>x.User)
                .FirstOrDefaultAsync(x => x.InvoiceNumber == invoiceNumber);

            if (invoice is null)
                throw new ApiException($"Invoice with number : '{invoiceNumber}' not found in our system");

            return _mapper.Map<InvoiceResponse>(invoice);
        }
    }
}