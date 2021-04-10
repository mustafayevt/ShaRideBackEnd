using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ShaRide.Application.Attributes;
using ShaRide.Application.Contexts;
using ShaRide.Application.Executables.Abstraction;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.Managers
{
    /// <summary>
    /// In charge of payment stuff.
    /// </summary>
    public sealed class PaymentManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager _userManager;
        private readonly IConfiguration _config;

        public PaymentManager(UserManager userManager, ApplicationDbContext dbContext, IConfiguration config)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _config = config;
        }
        
        /// <summary>
        /// Process payment stuff with discount and campaigns.
        /// </summary>
        /// <param name="ride"></param>
        public async Task ProcessPayment(Ride ride)
        {
            //Payment source balance processing.
            ProcessFromBalancePayment(ride);

            //Payment source cash processing.
            ProcessFromCashPayment(ride);

            //Discounts & campaigns processing.
            ProcessDiscountsAndCampaigns(ride);

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// This method responsible to all discount and campaign and coupon logic.
        /// No need to change logic of the method itself for adding new discount logic. <seealso cref="DiscountExecutableBase"/>
        /// </summary>
        /// <param name="ride"></param>
        private void ProcessDiscountsAndCampaigns(Ride ride)
        {
            //Getting all 'DiscountExecutableBase' inheritors.
            var discountTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(DiscountExecutableBase)));

            foreach (var type in discountTypes)
            {
                //Checking type is active or not via 'IsTypeActiveAttribute. if type is not active we don't process 
                if (type.GetCustomAttributes(typeof(IsTypeActiveAttribute)).FirstOrDefault() is IsTypeActiveAttribute {IsActive: false})
                    continue;

                //Creating discount executor instance
                var executor = Activator.CreateInstance(type, ride, _dbContext, _userManager, _config) as DiscountExecutableBase;

                executor?.Execute();
            }
        }

        /// <summary>
        /// Responsible to process cash payment logic.
        /// <remarks>Discount or coupon or campaign logic cannot be implement in this method. <see cref="ProcessDiscountsAndCampaigns"/></remarks>
        /// </summary>
        /// <param name="ride"></param>
        private void ProcessFromCashPayment(Ride ride)
        {
            var paymentSourceFromCashCarSeatComposition =
                ride.RideCarSeatComposition.Where(x => x.PaymentType == PaymentType.Cash).ToList();

            if (paymentSourceFromCashCarSeatComposition.Any())
            {
                foreach (var rideCarSeatComposition in paymentSourceFromCashCarSeatComposition)
                {
                    var amount = ride.PricePerSeat;

                    var driverPercentage =
                        _config.GetValue<decimal>("PaymentObject:FromCashProperties:DriverDeductPercentage"); // 15

                    // In here, we're deducting driver's income with default percentage.
                    ride.Driver.Balance -= (amount * driverPercentage) / 100;
                }
            }
        }

        /// <summary>
        /// Responsible to process balance payment logic
        /// <remarks>Discount or coupon or campaign logic cannot be implement in this method. <see cref="ProcessDiscountsAndCampaigns"/></remarks>
        /// </summary>
        /// <param name="ride"></param>
        private void ProcessFromBalancePayment(Ride ride)
        {
            var paymentSourceFromBalanceCarSeatComposition =
                ride.RideCarSeatComposition.Where(x => x.PaymentType == PaymentType.Balance).ToList();

            if (paymentSourceFromBalanceCarSeatComposition.Any())
            {
                foreach (var rideCarSeatComposition in paymentSourceFromBalanceCarSeatComposition)
                {
                    var amount = ride.PricePerSeat;

                    var driverPercentage = _config.GetValue<decimal>("PaymentObject:FromBalanceProperties:DriverDeductPercentage"); // 85

                    rideCarSeatComposition.Passenger.Balance -= amount;
                    // In here, we're deducting driver's income with default percentage.
                    ride.Driver.Balance += (amount * driverPercentage) / 100;
                }
            }
        }
    }
}