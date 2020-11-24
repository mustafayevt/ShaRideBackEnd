using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShaRide.Application.DTO.Request;
using ShaRide.Application.DTO.Response;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Managers;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Settings;

namespace ShaRide.Application.Services.Concrate
{
    public class AccountService : IAccountService
    {
        private readonly UserManager _userManager;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jwtSettings;
        private readonly IDateTimeService _dateTimeService;
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer _localizer;

        public AccountService(UserManager userManager,
            IOptions<JWTSettings> jwtSettings,
            IDateTimeService dateTimeService,
            IEmailService emailService,
            IVerificationCodeService verificationCodeService,
            IMapper mapper,
            IStringLocalizer<Resource> localizer)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _dateTimeService = dateTimeService;
            _emailService = emailService;
            _verificationCodeService = verificationCodeService;
            _mapper = mapper;
            _localizer = localizer;
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
                throw new ApiException(_localizer.GetString(LocalizationKeys.INVALID_CREDENTIALS,request.Phone));
            }

            var result = await _userManager.PasswordSignInAsync(request.Phone, request.Password);
            if (!result.Succeeded)
            {
                throw new ApiException(_localizer.GetString(LocalizationKeys.INVALID_CREDENTIALS,request.Phone));
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
                throw new ApiException(_localizer.GetString(LocalizationKeys.PHONE_ALREADY_TAKEN,mainPhone.Number));
            }

            mainPhone.IsConfirmed = true;

            var user = _mapper.Map<ApplicationUser>(request);
            user.Img = request.Attachment.Content.ToArray();
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
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
                throw new ApiException("Error while creating user");
            }
        }

        /// <summary>
        /// Sends verification code to user and returns client to the code.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<string> GetVerificationCode(string phoneNumber)
        {
            var userPhone = await _userManager.GetUserPhoneByPhoneNumber(phoneNumber);
            if (userPhone != null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.PHONE_ALREADY_CONFIRMED,phoneNumber));

            var confirmationCode = ConfirmationCodeHelper.GenerateConfirmationCode();
            var content = $"ShaRide Kod - {confirmationCode}";
            var result = await _verificationCodeService.SendVerificationCode(phoneNumber, content);
            return confirmationCode;
        }

        /// <summary>
        /// Generates jwt  
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            // var roles = await _userManager.GetRolesAsync(user);

            // var roleClaims = new List<Claim>();
            //
            // for (int i = 0; i < roles.Count; i++)
            // {
            //     roleClaims.Add(new Claim("roles", roles[i]));
            // }

            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Phones.FirstOrDefault(x => x.IsMain).Number),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.Id.ToString()),
                new Claim("ip", ipAddress)
            };
            // .Union(roleClaims);

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

        /// <summary>
        /// Sends verification email to user email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        private async Task<string> SendVerificationSms(ApplicationUser user, string origin)
        {
            var code = 123.ToString();
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id.ToString());
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }

        // /// <summary>
        // /// Generates refresh token
        // /// </summary>
        // /// <param name="ipAddress"></param>
        // /// <returns></returns>
        // private RefreshToken GenerateRefreshToken(string ipAddress)
        // {
        //     return new RefreshToken
        //     {
        //         Token = RandomTokenString(),
        //         Expires = DateTime.UtcNow.AddDays(7),
        //         Created = DateTime.UtcNow,
        //         CreatedByIp = ipAddress
        //     };
        // }

        // /// <summary>
        // /// Forgot user password operation
        // /// </summary>
        // /// <param name="model"></param>
        // /// <param name="origin"></param>
        // /// <returns></returns>
        // public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
        // {
        //     var account = await _userManager.FindByEmailAsync(model.Email);
        //
        //     // always return ok response to prevent email enumeration
        //     if (account == null) return;
        //
        //     var code = await _userManager.GeneratePasswordResetTokenAsync(account);
        //     var route = "api/account/reset-password/";
        //     var _enpointUri = new Uri(string.Concat($"{origin}/", route));
        //     var emailRequest = new EmailRequest()
        //     {
        //         Body = $"You reset token is - {code}",
        //         To = model.Email,
        //         Subject = "Reset Password",
        //     };
        //     await _emailService.SendAsync(emailRequest);
        // }
        //
        // /// <summary>
        // /// Reset user password operation.
        // /// </summary>
        // /// <param name="model"></param>
        // /// <returns></returns>
        // /// <exception cref="ApiException"></exception>
        // public async Task<string> ResetPassword(ResetPasswordRequest model)
        // {
        //     var account = await _userManager.FindByEmailAsync(model.Email);
        //     if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");
        //     var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
        //     if (result.Succeeded)
        //     {
        //         return "Password Resetted.";
        //     }
        //     else
        //     {
        //         throw new ApiException($"Error occured while reseting the password.");
        //     }
        // }
    }
}