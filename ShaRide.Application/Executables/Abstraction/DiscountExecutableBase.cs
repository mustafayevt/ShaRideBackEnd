using Microsoft.Extensions.Configuration;
using ShaRide.Application.Contexts;
using ShaRide.Application.Managers;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Executables.Abstraction
{
    /// <summary>
    /// Base of Discount related executable classes.
    /// The purpose of this class is that taking care of processing discount and coupon business logics.
    /// <example>
    /// <remarks>inheritors of this class always executes after payment and deducting process.</remarks>
    /// </example>
    /// </summary>
    public abstract class DiscountExecutableBase : ExecutableBase
    {
        protected readonly Ride Ride;
        protected readonly ApplicationDbContext DbContext;
        protected readonly UserManager UserManager;
        protected readonly IConfiguration Config;

        protected DiscountExecutableBase(Ride ride, ApplicationDbContext dbContext, UserManager userManager,
            IConfiguration config)
        {
            Ride = ride;
            DbContext = dbContext;
            UserManager = userManager;
            Config = config;
        }

        protected abstract void ProcessFromCashPayment(Ride ride);
        protected abstract void ProcessFromBalancePayment(Ride ride);
    }
}