using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Account;
using ShaRide.Application.DTO.Request.Sms;
using ShaRide.Application.DTO.Response.Account;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Managers;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Enums;
using ShaRide.Domain.Settings;

namespace ShaRide.Application.Services.Concrete
{
    public class AccountService : IAccountService
    {
        private readonly UserManager _userManager;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jwtSettings;
        private readonly IDateTimeService _dateTimeService;
        private readonly ISmsService _smsService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer _localizer;
        private readonly ApplicationDbContext _dbContext;

        public AccountService(UserManager userManager,
            IOptions<JWTSettings> jwtSettings,
            IDateTimeService dateTimeService,
            IEmailService emailService,
            IMapper mapper,
            IStringLocalizer<Resource> localizer,
            ApplicationDbContext dbContext, ISmsService smsService)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _dateTimeService = dateTimeService;
            _emailService = emailService;
            _mapper = mapper;
            _localizer = localizer;
            _dbContext = dbContext;
            _smsService = smsService;
        }

        /// <summary>
        /// User authentication
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var user = await _userManager.FindByPhoneAsync(request.Phone);
            if (user == null)
            {
                throw new ApiException(_localizer.GetString(LocalizationKeys.INVALID_CREDENTIALS, request.Phone));
            }

            var result = await _userManager.PasswordSignInAsync(request.Phone, request.Password);
            if (!result.Succeeded)
            {
                throw new ApiException(_localizer.GetString(LocalizationKeys.INVALID_CREDENTIALS, request.Phone));
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            AuthenticationResponse response = new AuthenticationResponse();
            response.Id = user.Id;
            response.IsVerified = user.Phones.FirstOrDefault(x => x.IsMain).IsConfirmed;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return response;
        }

        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="request"></param>
        /// <param name="origin"></param>
        /// <exception cref="ApiException"></exception>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        /// <exception cref="ValidationException"></exception>
        public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest request, string origin)
        {
            var mainPhone = request.Phones.First();
            var userWithSamePhone = await _userManager.FindByPhoneAsync(mainPhone.Number);
            if (userWithSamePhone != null)
            {
                throw new ApiException(_localizer.GetString(LocalizationKeys.PHONE_ALREADY_TAKEN, mainPhone.Number));
            }

            var user = _mapper.Map<User>(request);
            user.UserImages = new List<UserImage>
            {
                new UserImage
                {
                    Image = request.Attachment.Content.ToArray(),
                    Extension = request.Attachment.Extension
                }
            };
            var userResult = await _userManager.CreateAsync(user, request.Password);
            if (userResult.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());

                if (roleResult.Succeeded)
                {
                    JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
                    return new AuthenticationResponse
                    {
                        Id = user.Id,
                        IsVerified = false,
                        JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
                    };
                }
                else
                {
                    throw new ApiException("Error while adding role to the user");
                }
            }
            else
            {
                throw new ApiException("Error while creating user");
            }
        }

        /// <summary>
        /// Resets user password by phone.
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<int> ResetUserPassword(string phone, string newPassword)
        {
            User user = await _userManager.GetUserByPhoneNumber(phone);

            return await _userManager.ResetPassword(user, newPassword) == IdentityResult.Success ? 0 : -1;
        }

        /// <summary>
        /// Sends verification code to user for phone confirmation and returns code to the client.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<string> SendPhoneVerificationSms(string phoneNumber)
        {
            var userPhone = await _userManager.GetUserPhoneByPhoneNumber(phoneNumber);
            if (userPhone != null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.PHONE_ALREADY_CONFIRMED, phoneNumber));

            return await SendVerificationSmsToUser(phoneNumber);
        }

        /// <summary>
        /// Sends verification code to user for password reset and returns code to the client.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public async Task<string> SendPasswordResetSms(string phoneNumber)
        {
            var userPhone = await _userManager.GetUserPhoneByPhoneNumber(phoneNumber);
            if (userPhone is null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.PHONE_NOT_FOUND, phoneNumber));

            return await SendVerificationSmsToUser(phoneNumber);
        }

        /// <summary>
        /// Generates new code for verification.
        /// </summary>
        /// <returns></returns>
        private string GenerateConfirmationCode()
        {
            var confirmationCode = ConfirmationCodeHelper.GenerateConfirmationCode();
            return confirmationCode;
        }

        /// <summary>
        /// Sends verification sms to user with localization.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        private async Task<string> SendVerificationSmsToUser(string phoneNumber)
        {
            var confirmationCode = GenerateConfirmationCode();
            var smsBody = _localizer.GetString(LocalizationKeys.CONFIRMATION_SMS, confirmationCode);
            var result = await _smsService.SendSms(new SendSmsRequest(phoneNumber, smsBody));
            return confirmationCode;
        }

        /// <summary>
        /// Gets user thumbnail photo.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public async Task<UserImage> GetUserThumbnailPhoto(int userId)
        {
            var userImage = await _dbContext.Users.Where(x => x.IsRowActive).Include(x=>x.UserImages).FirstOrDefaultAsync(x => x.Id == userId);
            if(!_dbContext.Users.Where(x=>x.IsRowActive).Any(x=>x.Id == userId))
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND]);

            return userImage.UserImages.FirstOrDefault(x => x.IsRowActive);
        }

        /// <summary>
        /// Generates jwt  
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<JwtSecurityToken> GenerateJWToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Phones.FirstOrDefault(x => x.IsMain).Number),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.NameId, string.Concat(user.Name, " - ", user.Surname)),
                    new Claim("uid", user.Id.ToString()),
                    new Claim("ip", ipAddress)
                }
                .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        /// <summary>
        /// Generates token.
        /// </summary>
        /// <returns></returns>
        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}