using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.Extensions.Options;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Sms;
using ShaRide.Application.Helpers;
using ShaRide.Application.Services.Interface;

namespace ShaRide.Application.Services.Concrete
{
    public class SmsService : ISmsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<TheTexting> _textingOption;
        private readonly ApplicationDbContext _dbContext;

        public SmsService(IOptions<TheTexting> textingOption, IHttpClientFactory httpClientFactory, ApplicationDbContext dbContext)
        {
            _textingOption = textingOption;
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
        }

        public async Task<int> SendSms(SendSmsRequest request)
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(_textingOption.Value.BaseUrl);
                
                var url =
                    $"rest/sms/json/Message/Send?api_key={_textingOption.Value.ApiKey}&api_secret={_textingOption.Value.ApiSecret}&from={_textingOption.Value.From}&to={request.PhoneNumber.Replace("+","")}&text={request.MessageBody}&type=text";
                
                var response = await httpClient.GetAsync(url);
                
                var result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                throw new ApiException(e);
            }

            return 0;
        }

        public async Task<int> SendSmsToAllPotentialClients(string body)
        {
            var potentialClients = _dbContext.PotentialClientNumbers.Where(x => x.IsRowActive);

            foreach (var potentialClientNumber in potentialClients)
            {
                await SendSms(new SendSmsRequest(potentialClientNumber.Phone, body));
            }

            return 0;
        }

        public async Task<int> SendSmsToAllOurClients(string body)
        {
            var allClientsMainPhoneNumber =
                _dbContext.UserPhones.Where(x => x.IsConfirmed && x.IsMain && x.IsRowActive);

            foreach (var userPhone in allClientsMainPhoneNumber)
            {
                await SendSms(new SendSmsRequest(userPhone.Number, body));
            }

            return 0;
        }
    }
}