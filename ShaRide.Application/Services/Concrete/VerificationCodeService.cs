using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ShaRide.Application.Helpers;
using ShaRide.Application.Services.Interface;

namespace ShaRide.Application.Services.Concrete
{
    public class VerificationCodeService : IVerificationCodeService
    {
        private readonly IOptions<TheTexting> _textingCredentials;
        private readonly IHttpClientFactory _httpClientFactory;

        public VerificationCodeService(IOptions<TheTexting> textingCredentials , IHttpClientFactory httpClientFactory)
        {
            this._textingCredentials = textingCredentials;
            this._httpClientFactory = httpClientFactory;
        }

        public async Task<int> SendVerificationCode(string to, string content, string senderId = "ShaRide")
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            string url = $"https://www.thetexting.com/rest/sms/json/message/send?api_key={_textingCredentials.Value.ApiKey}&api_secret={_textingCredentials.Value.ApiSecret}&to={to}&text={content}&senderId={senderId}";
            var response = await httpClient.GetAsync(url);
            return (int) response.StatusCode;
        }
    }
}