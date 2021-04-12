using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShaRide.Application.Attributes;
using ShaRide.Application.Contexts;
using ShaRide.Application.Executables.Abstraction;
using ShaRide.Application.Managers;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.Executables.Concrete.DiscountExecutors
{
    /// <summary>
    /// Executable that takes care our first 500 client coupon thing.
    /// </summary>
    [IsTypeActive(true)]
    public class First500UserCouponExecutor : DiscountExecutableBase
    {
        public First500UserCouponExecutor(Ride ride, ApplicationDbContext dbContext, UserManager userManager, IConfiguration config) : base(ride, dbContext, userManager, config)
        {
        }

        private const int FREE_SEAT_COUNT = 20;
        private int userSoldSeatsCount;
        
        public override void Execute()
        {
            
            var userHasCoupon = DbContext
                .Users
                .Include(x => x.UserRoleComposition)
                .OrderBy(x => x.CreatedTimestamp)
                .Where(x => x.UserRoleComposition.All(y => y.Role.RoleName != Roles.Admin.ToString()))
                .Take(500)
                .FirstOrDefault(x => x.Id.Equals(Ride.DriverId)) != null;

            if (!userHasCoupon)
                return;

            userSoldSeatsCount = DbContext.RideCarSeatCompositions.Include(x => x.Ride)
                .Count(x => x.SeatStatus == SeatStatus.Sold && x.PassengerId.HasValue);

            if (userSoldSeatsCount >= FREE_SEAT_COUNT)
                return;
            
            ProcessFromBalancePayment(Ride);
            ProcessFromCashPayment(Ride);
            
        }
        protected override void ProcessFromCashPayment(Ride ride)
        {
            var paymentSourceFromCashCarSeatComposition =
                ride.RideCarSeatComposition.Where(x => x.PaymentType == PaymentType.Cash).ToList();

            if (paymentSourceFromCashCarSeatComposition.Any())
            {
                foreach (var rideCarSeatComposition in paymentSourceFromCashCarSeatComposition)
                {
                    if(userSoldSeatsCount++ > FREE_SEAT_COUNT)
                        return;
                    
                    var amount = ride.PricePerSeat;

                    var driverPercentage =
                        Config.GetValue<decimal>("PaymentObject:FromCashProperties:DriverDeductPercentage"); // 15

                    // In here, we're increase driver's income with default percentage.
                    ride.Driver.Balance += (amount * (100 - driverPercentage)) / 100;
                }
            }
        }

        protected override void ProcessFromBalancePayment(Ride ride)
        {
            var paymentSourceFromBalanceCarSeatComposition =
                ride.RideCarSeatComposition.Where(x => x.PaymentType == PaymentType.Balance).ToList();

            if (paymentSourceFromBalanceCarSeatComposition.Any())
            {
                foreach (var rideCarSeatComposition in paymentSourceFromBalanceCarSeatComposition)
                {
                    if(userSoldSeatsCount++ > FREE_SEAT_COUNT)
                        return;

                    var amount = ride.PricePerSeat;

                    var driverPercentage = Config.GetValue<decimal>("PaymentObject:FromBalanceProperties:DriverDeductPercentage"); // 85

                    // In here, we're increase driver's income with default percentage.
                    ride.Driver.Balance += (amount * (100 - driverPercentage)) / 100;
                }
            }
        }
    }
}