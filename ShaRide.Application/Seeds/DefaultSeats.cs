using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ShaRide.Application.Contexts;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Seeds
{
    /// <summary>
    /// Seeds default seats to db.
    /// </summary>
    public static class DefaultSeats
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            if (!dbContext.Seats.Any())
            {
                for (short i = 1; i <= 10; i++)
                {
                    for (short j = 1; j <= 100; j++)
                    {
                        var seat = new Seat
                        {
                            xCordinant = i,
                            yCordinant = j
                        };
                        await dbContext.Seats.AddAsync(seat);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}