using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.Extensions.Options;
using ShaRide.Application.DTO.Request.Sms;
using ShaRide.Application.Helpers;
using ShaRide.Application.Services.Interface;

namespace ShaRide.Application.Services.Concrete
{
    public class SmsService : ISmsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<TheTexting> _textingOption;

        public SmsService(IOptions<TheTexting> textingOption, IHttpClientFactory httpClientFactory)
        {
            _textingOption = textingOption;
            _httpClientFactory = httpClientFactory;
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
    }
}