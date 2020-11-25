using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShaRide.Application.Managers;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.Seeds
{
    /// <summary>
    /// Seeds Default admin users.
    /// </summary>
    public static class DefaultAdminUsers
    {
        public static async Task SeedAsync(UserManager userManager)
        {
            var defaultUserPhone = "+994703353383";
            var defaultUser = new User
            {
                Name = "Saleh",
                Surname = "Abdullabeyli",
                Phones = new List<UserPhone>
                {
                    new UserPhone
                    {
                        Number = defaultUserPhone,
                        IsConfirmed = true,
                        IsMain = true
                    }
                }
            };

            if (!userManager.Users.Include(x => x.Phones).Any(y => y.Phones.Any(h => h.Number == defaultUserPhone)))
            {
                await userManager.CreateAsync(defaultUser, "S@l@m2901");
                await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
            }
        }
    }
}