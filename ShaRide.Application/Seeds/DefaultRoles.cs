using System.Threading.Tasks;
using ShaRide.Application.Managers;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.Seeds
{
    /// <summary>
    /// Seeds default roles.
    /// </summary>
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager userManager)
        {
            await userManager.CreateRoleAsync(new Role(Roles.Admin.ToString()));
            await userManager.CreateRoleAsync(new Role(Roles.Basic.ToString()));
        }
    }
}