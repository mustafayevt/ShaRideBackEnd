using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Response.Accounting;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.Services.Concrete
{
    public class AccountingService : IAccountingService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public AccountingService(ApplicationDbContext dbContext, IAuthenticatedUserService authenticatedUserService)
        {
            _dbContext = dbContext;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<CarAccountingGeneralInfoResponse> GetCarAccounting()
        {
            //TODO: do include improvements
            var driverRides = await _dbContext
                .Rides
                .Include(x => x.Driver)
                .ThenInclude(x => x.Phones)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.CarSeatComposition)
                .ThenInclude(x => x.Seat)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.Passenger)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.CarSeatComposition)
                .ThenInclude(x => x.Car)
                .ThenInclude(x => x.CarModel)
                .ThenInclude(x => x.BanType)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.CarSeatComposition)
                .ThenInclude(x => x.Car)
                .ThenInclude(x => x.CarModel)
                .ThenInclude(x => x.CarBrand)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.CarSeatComposition)
                .ThenInclude(x => x.Car)
                .ThenInclude(x => x.CarImages)
                .Include(x => x.RideLocationPointComposition)
                .ThenInclude(x => x.LocationPoint)
                .ThenInclude(x => x.Location)
                .Include(x => x.RestrictionRideComposition)
                .ThenInclude(x => x.Restriction)
                .Where(x => x.IsRowActive && x.DriverId.Equals(_authenticatedUserService.UserId) &&
                            x.RideState == RideState.Finished)
                .ToListAsync();

            var driverRidesGroupingByCar =
                driverRides.GroupBy(x => x.RideCarSeatComposition.Select(y => y.CarSeatComposition.CarId).First());

            var carAccountingResponses = new List<CarAccountingResponse>();
            foreach (var byCar in driverRidesGroupingByCar)
            {
                //Detailed
                IList<CarAccountingDetailedResponse> detailed = byCar.Select(y =>
                {
                    var detailedSumIncome = y.RideCarSeatComposition
                                                .Count(j => j.IsRowActive && j.SeatStatus == SeatStatus.Sold
                                                                          && j.PassengerId.HasValue
                                                                          && j.PassengerId != _authenticatedUserService
                                                                              .UserId) *
                                            y.PricePerSeat;
                    return new CarAccountingDetailedResponse
                    {
                        Date = y.StartDate,
                        From = y.RideLocationPointComposition
                            .SingleOrDefault(h => h.LocationPointType == LocationPointType.StartPoint)?.LocationPoint
                            .Location.Name,
                        To = y.RideLocationPointComposition
                            .SingleOrDefault(h => h.LocationPointType == LocationPointType.FinishPoint)?.LocationPoint
                            .Location.Name,
                        SumIncome = detailedSumIncome,
                        Profit = detailedSumIncome * 85 / 100,
                        Commission = detailedSumIncome * 15 / 100,
                    };
                }).ToList();

                var sumIncome = detailed.Sum(x => x.SumIncome);

                var car = driverRides
                    .FirstOrDefault(x => x.RideCarSeatComposition.Any(y => y.CarSeatComposition.CarId == byCar.Key))
                    !.RideCarSeatComposition.FirstOrDefault(x => x.CarSeatComposition.CarId == byCar.Key)
                    !.CarSeatComposition.Car;
                
                carAccountingResponses.Add(new CarAccountingResponse
                {
                    Detailed = detailed,
                    SumIncome = sumIncome,
                    Profit = sumIncome * 85 / 100,
                    CarTitle = $"{car.CarModel.CarBrand.Title}  {car.CarModel.Title}",
                    RegisterNumber = car.RegisterNumber,
                    CarBanAsset = car.CarModel.BanType.AssetPath
                });
            }

            var response = new CarAccountingGeneralInfoResponse
            {
                CarAccountings = carAccountingResponses,
                Profit = carAccountingResponses.Sum(x => x.Profit),
                SumIncome = carAccountingResponses.Sum(x => x.SumIncome)
            };

            return response;
        }

        public async Task<IList<PaymentDetailResponse>> GetPaymentDetails()
        {
            var userPayments = await _dbContext
                .Invoices
                .Include(x => x.User)
                .ThenInclude(x => x.Phones)
                .Where(x => x.IsRowActive && x.UserId.Equals(_authenticatedUserService.UserId))
                .ToListAsync();

            return userPayments.Select(x =>
            {
                return new PaymentDetailResponse
                {
                    Amount = x.Amount,
                    Fullname = $"{x.User.Name} {x.User.Surname}",
                    Id = x.Id,
                    Number = x.InvoiceNumber,
                    Phone = x.User.Phones.FirstOrDefault(y => y.IsRowActive && y.IsMain && y.IsConfirmed)?.Number,
                    DateTime = x.InvoiceRegisterDate,
                    UserUniqueKey = x.User.UserUniqueKey,
                    PaymentProvider = "Unknowing" // For future, we need to change provider.
                };
            }).ToList();
        }
    }
}