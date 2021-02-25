using System.Linq;
using System.Threading.Tasks;
using ShaRide.Application.Constants;
using ShaRide.Application.Contexts;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Seeds
{
    /// <summary>
    /// Seeds Default admin users.
    /// </summary>
    public static class DefaultCounters
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (dbContext.Counters.Any())
                return;
            
            var counter = new Counter
            {
                Name = CounterConstants.USER_UNIQUE_KEY_COUNTER,
                Value = 100000,
                Description = "For generate new user unique key."
            };

            await dbContext.Counters.AddAsync(counter);
            await dbContext.SaveChangesAsync();
        }
    }
}