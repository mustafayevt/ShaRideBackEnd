using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShaRide.Application.Contexts;
using ShaRide.Application.Services.Interface;

namespace ShaRide.Application.Services.Concrete
{
    public class CounterService : ICounterService
    {
        private readonly ApplicationDbContext _dbContext;

        public CounterService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetNextCounter(string counterName)
        {
            var counter = await _dbContext.Counters.AsTracking().FirstOrDefaultAsync(x => x.Name == counterName);

            if (counter is null)
                throw new ArgumentException("Counter is not found");

            var nextCounterValue = counter.Value++;

            await _dbContext.SaveChangesAsync();

            return nextCounterValue;
        }
    }
}