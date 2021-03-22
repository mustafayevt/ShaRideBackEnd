using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShaRide.Application.Constants;
using ShaRide.Application.Contexts;
using ShaRide.Application.Services.Concrete;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.Managers
{
    /// <summary>
    /// User manager.
    /// </summary>
    public class UserManager
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly ICounterService _counterService;
        private readonly ApplicationDbContext _dbContext;

        public IQueryable<User> Users => _dbContext.Users.AsQueryable();

        public UserManager(ApplicationDbContext dbContext, IAuthenticatedUserService authenticatedUserService, ICounterService counterService)
        {
            _dbContext = dbContext;
            _authenticatedUserService = authenticatedUserService;
            _counterService = counterService;
        }

        /// <summary>
        /// Finds user by user phone.
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task<User> FindByPhoneAsync(string phone)
        {
            var userPhone = await _dbContext.UserPhones.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Number == phone);
            return userPhone?.User;
        }

        /// <summary>
        /// Returns user by unique key.
        /// </summary>
        /// <param name="uniqueKey"></param>
        /// <returns></returns>
        public async Task<User> FindByUniqueKey(string uniqueKey)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.UserUniqueKey == uniqueKey && x.IsRowActive);
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
        /// Gets user by given phone number.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>null not founds</returns>
        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            return (await _dbContext.UserPhones
                .Include(x => x.User)
                .FirstOrDefaultAsync(y => y.Number == phoneNumber))?.User;
        }

        /// <summary>
        /// Creates user and save to the database.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            try
            {
                var passwordHash = PasswordHasher.HashPassword(password);
                user.PasswordHash = passwordHash;
                user.UserUniqueKey = await GetNewUserUniqueKey();
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed();
            }
        }

        /// <summary>
        /// Sets new password to user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<IdentityResult> ResetPassword(User user, string newPassword)
        {
            try
            {
                var passwordHash = PasswordHasher.HashPassword(newPassword);
                
                _dbContext.Users.Attach(user);
                
                user.PasswordHash = passwordHash;
                
                await _dbContext.SaveChangesAsync();
                
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed();
            }
        }

        /// <summary>
        /// Adds role to user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<IdentityResult> AddToRoleAsync(User user, string roleName)
        {
            if (!Enum.TryParse(typeof(Roles), roleName, false, out var roleEnum))
                throw new ArgumentException("role name is not correct");

            var dbUser = await FindByPhoneAsync(user.Phones.First().Number);
            var dbRole = await _dbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == roleEnum.ToString());

            var composition = new UserRoleComposition(dbUser.Id, dbRole.Id);

            await _dbContext.UserRoleComposition.AddAsync(composition);

            await _dbContext.SaveChangesAsync();

            return IdentityResult.Success;
        }

        /// <summary>
        /// Adds role to the db.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateRoleAsync(Role role)
        {
            if (await _dbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == role.RoleName) != null)
                return IdentityResult.Failed();

            await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();

            return IdentityResult.Success;
        }

        /// <summary>
        /// Returns role by name.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<Role> GetRoleByName(string roleName)
        {
            return await _dbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == roleName);
        }

        /// <summary>
        /// Gets user roles as string list.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<string>> GetRolesAsync(User user)
        {
            return _dbContext.UserRoleComposition
                .Include(x => x.Role)
                .Where(x => x.UserId == user.Id)
                .Select(x => x.Role.RoleName).ToList();
        }

        /// <summary>
        /// Gets current logged in user from db.
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetCurrentUser()
        {
            return await _dbContext.Users.FindAsync(_authenticatedUserService.UserId);
        }

        /// <summary>
        /// Gets user by id from db.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool TryGetUserById(int userId, out User user )
        {
            user = _dbContext.Users.Find(userId);
            return user != null;
        }

        public async Task<string> GetNewUserUniqueKey()
        {
            var nextCounter = await _counterService.GetNextCounter(CounterConstants.USER_UNIQUE_KEY_COUNTER);

            return FormatUserUniqueKey(nextCounter);
        }

        private string FormatUserUniqueKey(int counter)
        {
            return $"SH{counter}";
        }
    }
}