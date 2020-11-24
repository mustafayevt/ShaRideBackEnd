using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShaRide.Application.Contexts;
using ShaRide.Application.Services.Concrate;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Managers
{
    /// <summary>
    /// User manager.
    /// </summary>
    public class UserManager
    {
        private readonly ApplicationDbContext _dbContext;

        public UserManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Finds user by user phone.
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> FindByPhoneAsync(string phone)
        {
            var userPhone =  await _dbContext.UserPhones.Include(x => x.User).FirstOrDefaultAsync(x => x.Number == phone);
            return userPhone?.User;
        }

        /// <summary>
        /// Checks user password and returns SignInResult.
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="AuthenticationException"></exception>
        public async Task<SignInResult> PasswordSignInAsync(string phone, string password)
        {
            try
            {
                var user = _dbContext.Users.First(x => x.Phones.Any(y => y.Number == phone));
                var passwordHash = PasswordHasher.HashPassword(password);
                if (user.PasswordHash != passwordHash)
                {
                    throw new AuthenticationException("Password is wrong");
                }

                return SignInResult.Success;
            }
            catch (Exception e)
            {
                return SignInResult.Failed;
            }
        }

        /// <summary>
        /// Gets user phone by given phone number.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<UserPhone> GetUserPhoneByPhoneNumber(string phoneNumber)
        {
            return await _dbContext.UserPhones.FirstOrDefaultAsync(x => x.Number == phoneNumber);
        }

        /// <summary>
        /// Creates user and save to the database.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            try
            {
                var passwordHash = PasswordHasher.HashPassword(password);
                user.PasswordHash = passwordHash;
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed();
            }
        }
    }
}