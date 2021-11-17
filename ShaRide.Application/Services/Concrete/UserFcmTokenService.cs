using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.UserFcmToken;
using ShaRide.Application.DTO.Response.UserFcmToken;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Managers;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Settings;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ShaRide.Application.Services.Concrete
{
    public class UserFcmTokenService : IUserFcmTokenService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer _localizer;
        private readonly UserManager _userManager;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<FcmSettings> _fcmSettings;

        public UserFcmTokenService(ApplicationDbContext dbContext, IMapper mapper, IStringLocalizer<Resource> localizer, UserManager userManager, IHttpClientFactory httpClientFactory, IOptions<FcmSettings> fcmSettings)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _localizer = localizer;
            _userManager = userManager;
            _httpClientFactory = httpClientFactory;
            _fcmSettings = fcmSettings;
        }

        public async Task<UserFcmTokenResponse> InsertToken(UserFcmTokenInsertRequest request)
        {
            var existingToken = await _dbContext.UserFcmTokens.AsTracking().FirstOrDefaultAsync(x => x.IsRowActive && x.Token == request.Token && request.UserId == x.UserId && request.DeviceId == x.DeviceId);

            if (existingToken != null)
                return _mapper.Map<UserFcmTokenResponse>(existingToken);

            _dbContext.UserFcmTokens.RemoveRange(_dbContext.UserFcmTokens.Where(x => x.DeviceId.Equals(request.DeviceId)));

            var userFcmToken = _mapper.Map<UserFcmToken>(request);

            if (!_userManager.TryGetUserById(request.UserId, out _))
                throw new ApiException(_localizer.GetString(LocalizationKeys.NOT_FOUND, request.UserId));

            await _dbContext.UserFcmTokens.AddAsync(userFcmToken);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserFcmTokenResponse>(userFcmToken);
        }

        public async Task<UserFcmTokenResponse> UpdateToken(UserFcmTokenUpdateRequest request)
        {
            var userFcmToken = await _dbContext.UserFcmTokens.AsTracking().FirstOrDefaultAsync(x => x.IsRowActive && x.Token == request.OldToken);

            if (userFcmToken is null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.NOT_FOUND, request.OldToken));

            if (!_userManager.TryGetUserById(request.UserId, out _))
                throw new ApiException(_localizer.GetString(LocalizationKeys.NOT_FOUND, request.UserId));

            userFcmToken.Token = request.NewToken;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserFcmTokenResponse>(userFcmToken);
        }

        public async Task<int> DeleteToken(UserFcmTokenDeleteRequest request)
        {
            var userFcmToken = await _dbContext.UserFcmTokens.SingleOrDefaultAsync(x => x.IsRowActive && x.UserId.Equals(request.UserId) && x.Token.Equals(request.Token));

            if (userFcmToken is null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.NOT_FOUND, request.Token));

            if (!_userManager.TryGetUserById(request.UserId, out _))
                throw new ApiException(_localizer.GetString(LocalizationKeys.NOT_FOUND, request.UserId));

            _dbContext.Entry(userFcmToken).State = EntityState.Deleted;

            await _dbContext.SaveChangesAsync();

            return 0;
        }

        public async Task<int> SendNotificationToUser(FcmNotificationContract contract)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("BEARER", _fcmSettings.Value.BearerToken);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _fcmSettings.Value.Url)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(contract),
                        Encoding.UTF8,
                        "application/json")
                };


                var responseMessage = await httpClient.SendAsync(request);

                if (!responseMessage.IsSuccessStatusCode)
                    throw new ApiException("Error while sending request to FCM service");
            }

            return 0;
        }
    }
}