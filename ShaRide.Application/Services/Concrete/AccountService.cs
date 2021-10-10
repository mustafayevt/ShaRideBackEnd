using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Account;
using ShaRide.Application.DTO.Request.Feedback;
using ShaRide.Application.DTO.Request.Sms;
using ShaRide.Application.DTO.Request.UserFcmToken;
using ShaRide.Application.DTO.Response.Account;
using ShaRide.Application.DTO.Response.Car;
using ShaRide.Application.DTO.Response.Feedback;
using ShaRide.Application.DTO.Response.Phone;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Managers;
using ShaRide.Application.Pagination;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Enums;
using ShaRide.Domain.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShaRide.Application.Services.Concrete
{
    public class AccountService : IAccountService
    {
        private readonly UserManager _userManager;
        private readonly JWTSettings _jwtSettings;
        private readonly ISmsService _smsService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer _localizer;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRatingService _userRatingService;
        private readonly IOptions<FcmNotificationContract> _fcmNotificationContract;
        private readonly IUserFcmTokenService _userFcmTokenService;

        public AccountService(UserManager userManager,
            IOptions<JWTSettings> jwtSettings,
            IMapper mapper,
            IStringLocalizer<Resource> localizer,
            ApplicationDbContext dbContext, ISmsService smsService,
            IUserRatingService userRatingService,
            IOptions<FcmNotificationContract> fcmNotificationContract,
            IUserFcmTokenService userFcmTokenService)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _mapper = mapper;
            _localizer = localizer;
            _dbContext = dbContext;
            _smsService = smsService;
            _userRatingService = userRatingService;
            _fcmNotificationContract = fcmNotificationContract;
            _userFcmTokenService = userFcmTokenService;
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
            AuthenticationResponse response = new AuthenticationResponse
            {
                Id = user.Id,
                IsVerified = user.Phones.FirstOrDefault(x => x.IsMain).IsConfirmed,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };

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
            if (request.Attachment != null && request.Attachment.Content != null && request.Attachment.Extension != null)
            {
                user.UserImages = new List<UserImage>
                {
                    new UserImage
                    {
                        Image = request.Attachment.Content.ToArray(),
                        Extension = request.Attachment.Extension
                    }
                };
            }

            var userResult = await _userManager.CreateAsync(user, request.Password);
            if (userResult.Succeeded)
            {
                //Verify user phone
                var userPhone = _dbContext.UserPhones.AsTracking()
                    .FirstOrDefault(x => x.IsRowActive && x.UserId.Equals(user.Id) && x.IsMain);
                if (userPhone != null)
                {
                    userPhone.IsConfirmed = true;
                }

                var roleResult = await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                var isBakcellNumber = await BAKCELL.IsBakCellNUmber(userPhone.Number);
                if (isBakcellNumber)
                {
                    var baccellUers = _dbContext.Users
                        .Count(x => x.Phones.Any(p => p.Number.Contains("99455") || p.Number.Contains("99499")));
                    if (baccellUers < 1000)
                    {
                        user.Balance += 3;
                    }
                    else if (baccellUers < 2000)
                    {
                        user.Balance += 2;
                    }
                    else if (baccellUers < 3000)
                    {
                        user.Balance += 1;
                    }

                }
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                if (roleResult.Succeeded)
                {
                    JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
                    return new AuthenticationResponse
                    {
                        Id = user.Id,
                        IsVerified = true,
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
        /// User registration
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="ApiException"></exception>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        /// <exception cref="ValidationException"></exception>
        public async Task<UserResponse> UpdateUserInfoAsync(UpdateUserInfoRequest request)
        {
            var mainPhone = request.Phones.First();
            var userWithSamePhone = await _userManager.FindByPhoneAsync(mainPhone.Number, request.UserId);
            if (userWithSamePhone != null)
            {
                throw new ApiException(_localizer.GetString(LocalizationKeys.PHONE_ALREADY_TAKEN, mainPhone.Number));
            }

            var user = await _userManager.Users
                .Include(u => u.UserImages)
                .Include(u => u.Phones)
                .FirstOrDefaultAsync(u => u.Id == request.UserId);

            if (request.Attachment != null &&
                request.Attachment.Content != null &&
                request.Attachment.Extension != null &&
                !request.Attachment.Id.HasValue)
            {
                _dbContext.UserImage.RemoveRange(user.UserImages);

                user.UserImages = new List<UserImage>
                {
                    new UserImage
                    {
                        Image = request.Attachment.Content.ToArray(),
                        Extension = request.Attachment.Extension
                    }
                };
            }
            else
            {
                _dbContext.UserImage.RemoveRange(user.UserImages);
            }

            user.Name = request.FirstName;
            user.Surname = request.LastName;
            try
            {
                if (user.Phones.Select(p => p.Number).All(request.Phones.Select(p => p.Number).Contains))
                {
                    _dbContext.UserPhones.RemoveRange(user.Phones);
                    user.Phones = request.Phones.Select(p => _mapper.Map<UserPhone>(p)).ToList();
                    user.Phones.First(p => p.IsMain).IsConfirmed = true;
                }
            }
            catch (Exception ex) 
            {

                throw;
            }
           

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserResponse>(user);
        }

        /// <summary>
        /// Resets user password by phone.
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<AuthenticationResponse> ResetUserPassword(string phone, string newPassword)
        {
            User user = await _userManager.GetUserByPhoneNumber(phone);

            if (user is null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.PHONE_NOT_FOUND, phone));

            var passwordResetResult = await _userManager.ResetPassword(user, newPassword);

            if (!passwordResetResult.Succeeded)
                throw new ApiException(_localizer.GetString(LocalizationKeys.INVALID_CREDENTIALS, phone));

            var authResponse = await AuthenticateAsync(new AuthenticationRequest
            {
                Phone = phone,
                Password = newPassword
            }, null);

            return authResponse;
        }

        /// <summary>
        /// Saves feedback from user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<FeedbackResponse> Feedback(InsertFeedbackRequest request)
        {
            var feedback = _mapper.Map<Feedback>(request);

            await _dbContext.Feedbacks.AddAsync(feedback);

            await _dbContext.SaveChangesAsync();

            await _dbContext.Attach(feedback).Reference(x => x.CreatedByUser).Query().Include(x => x.Phones)
                .LoadAsync();

            return _mapper.Map<FeedbackResponse>(feedback);
        }

        /// <summary>
        /// Returns all active feedbacks.
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<FeedbackResponse>> GetAllFeedbacks()
        {
            var feedbacks = await _dbContext.Feedbacks.Include(x => x.CreatedByUser).ThenInclude(x => x.Phones)
                .Where(x => x.IsRowActive).ToListAsync();

            return _mapper.Map<ICollection<FeedbackResponse>>(feedbacks);
        }

        public async Task<decimal> GetCurrentUserBalance()
        {
            var user = await _userManager.GetCurrentUser();

            if (user == null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.INVALID_CREDENTIALS));

            return user.Balance;
        }

        public Task<decimal> GetUserBalance(int userId)
        {
            if (!_userManager.TryGetUserById(userId, out var user))
                throw new ApiException(_localizer.GetString(LocalizationKeys.NOT_FOUND, userId));

            return Task.FromResult(user.Balance);
        }

        public async Task<int> InsertPotentialClientPhone(InsertPotentialClientPhoneRequest request)
        {
            var phone = request.Phone.Replace("+", "").Replace("-", "").Replace(" ", "");

            if (string.IsNullOrEmpty(phone))
                return 1;

            var isExistingPhone = _dbContext.PotentialClientNumbers.Any(x => x.Phone.Equals(phone));

            if (isExistingPhone)
                return 1;

            await _dbContext.PotentialClientNumbers.AddAsync(new PotentialClientNumber
            {
                Phone = phone
            });

            await _dbContext.SaveChangesAsync();


            return 0;
        }

        public async Task<int> DeactivateUser(DeactivateUserRequest request)
        {
            if (!_userManager.TryGetUserById(request.UserId, out User user))
                throw new ApiException(_localizer.GetString(LocalizationKeys.NOT_FOUND, request.UserId));

            user = await _userManager.DeactivateUser(user, request);


            try
            {
                var userFcmToken = await _dbContext.UserFcmTokens.FirstOrDefaultAsync(x => x.IsRowActive && x.UserId == user.Id);

                var fcmContract = _fcmNotificationContract.Value;

                fcmContract.data.ActionInApp = $"User deaktivated";
                var notificationBody = _localizer.GetString(LocalizationKeys.UserDeactivated,
                new string[] {
                    $"{user.Name} {user.Surname}",
                    request.ExpirationDate.ToString("dd.MM.yyyy"),
                    request.Reason
                });

                fcmContract.notification = new FcmNotificationContract.Notification(notificationBody, fcmContract.data.Title);
                fcmContract.registration_ids = new List<string>() { userFcmToken.Token };
                await _userFcmTokenService.SendNotificationToUser(fcmContract);
            }
            catch (Exception)
            {

                throw;
            }




            //var smsBody = _localizer.GetString(LocalizationKeys.UserDeactivated, 
            //    new List<string> {
            //        $"{user.Name} {user.Surname}",  
            //        request.ExpirationDate.ToString("dd.MM.yyyy"),
            //        request.Reason
            //    });
            //await _smsService.SendSms(new SendSmsRequest(user.Phones.FirstOrDefault(p=>p.IsMain).Number, smsBody));

            return 0;
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
            await _smsService.SendSms(new SendSmsRequest(phoneNumber, smsBody));
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
            if (!_userManager.TryGetUserById(userId, out User user))
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND]);

            await _dbContext.Attach(user).Collection(x => x.UserImages).LoadAsync();

            if (!_dbContext.Users.Where(x => x.IsRowActive).Any(x => x.Id == userId))
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND]);

            return user.UserImages.FirstOrDefault(x => x.IsRowActive);
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
                    new Claim(JwtRegisteredClaimNames.Sub, user.Phones.FirstOrDefault(x => x.IsMain)?.Number),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.NameId, user.Name),
                    new Claim(JwtRegisteredClaimNames.FamilyName, user.Surname),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserUniqueKey),
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
        //private string RandomTokenString()
        //{
        //    using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        //    var randomBytes = new byte[40];
        //    rngCryptoServiceProvider.GetBytes(randomBytes);
        //    // convert random bytes to hex string
        //    return BitConverter.ToString(randomBytes).Replace("-", "");
        //}


        /// <summary>
        /// Returns users according to filter request  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PaginatedList<UserFilterResponse>> AllUsers(UserFilterRequest request)
        {
            var users = _dbContext.Users
                .Include(u => u.Phones)
                .Include(u => u.UserRoleComposition).ThenInclude(c => c.Role)
                .Include(u => u.UserCars).ThenInclude(c => c.BanType)
                .Include(u => u.UserCars).ThenInclude(c => c.CarModel).ThenInclude(c => c.CarBrand)
                .Include(u => u.UserCars).ThenInclude(c => c.CarSeatComposition).ThenInclude(c => c.Seat)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                users = users.Where(u => u.Name.ToLower().Contains(request.Name.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(request.Surname))
            {
                users = users.Where(u => u.Surname.ToLower().Contains(request.Surname.ToLower()));
            }

            if (request.FromCreationDate.HasValue)
            {
                users = users.Where(u => u.CreatedTimestamp >= request.FromCreationDate);
            }

            if (request.ToCreationDate.HasValue)
            {
                users = users.Where(u => u.CreatedTimestamp <= request.ToCreationDate);
            }

            if (request.Phones?.Count > 0)
            {
                users = users.Where(u => u.Phones.Select(p => p.Number).Any(p => p.Contains(request.Phones.FirstOrDefault())));
            }

            if (request.UserRoles?.Count > 0)
            {
                users = users.Where(u => u.UserRoleComposition.Select(p => p.Role.RoleName).Any(p => request.UserRoles.Contains(p)));
            }

            if (request.UserCars != null)
            {
                if (request.UserCars.ModelIds?.Count > 0)
                {
                    users = users.Where(u => u.UserCars.Select(c => c.CarModelId).Any(m => request.UserCars.ModelIds.Contains(m)));
                }

                if (request.UserCars.BanTypeIds?.Count > 0)
                {
                    users = users.Where(u => u.UserCars.Select(c => c.BanTypeId).Any(b => request.UserCars.BanTypeIds.Contains(b)));
                }

                if (request.UserCars.RegisterNumbers?.Count > 0)
                {
                    users = users.Where(u => u.UserCars.Select(c => c.RegisterNumber).Any(n => request.UserCars.RegisterNumbers.Contains(n)));
                }
            }

            if (request.FromBalance.HasValue)
            {
                users = users.Where(u => u.Balance >= request.FromBalance);
            }

            if (request.ToBalance.HasValue)
            {
                users = users.Where(u => u.Balance <= request.ToBalance);
            }

            if (users.Any())
            {
                var filteredUsers = await users.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

                var filteredUsersResponse = new List<UserFilterResponse>();

                foreach (var user in filteredUsers)
                {
                    var userResponse = new UserFilterResponse(user);
                    userResponse.Cars = user.UserCars.Select(c => _mapper.Map<CarResponse>(c)).ToList();
                    userResponse.Phones = user.Phones.Select(p => _mapper.Map<UserPhoneResponse>(p)).ToList();
                    userResponse.Rating = await _userRatingService.GetUserRating(user.Id);
                    filteredUsersResponse.Add(userResponse);
                }

                var result = new PaginatedList<UserFilterResponse>
                    (filteredUsersResponse, await users.CountAsync(), request.PageNumber, request.PageSize);

                return result;
            }
            return null;
        }
    }
}