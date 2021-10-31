using AutoMapper;
using ShaRide.Application.DTO.Response.Account;
using ShaRide.Application.DTO.Response.Car;
using ShaRide.Application.DTO.Response.Ride;
using ShaRide.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ShaRide.Application.ManualMapping
{
    public static class RideMap
    {
        public static ICollection<RideResponse> RidesToRideResponses(this IEnumerable<Ride> rides, IMapper mapper)
        {
            var rideResponses = new List<RideResponse>();

            foreach (var ride in rides)
            {
                var rideResponse = mapper.Map<RideResponse>(ride);
                rideResponse.Car =
                    mapper.Map<RideCarResponse>(ride.RideCarSeatComposition.FirstOrDefault()?.CarSeatComposition.Car);
                if (rideResponse.Car != null)
                {
                    rideResponse.Car.CarSeats = new List<CarSeatCompositionResponse>();

                    foreach (var rideCarSeatComposition in ride.RideCarSeatComposition)
                    {
                        var carSeatCompositionResponse = new CarSeatCompositionResponse
                        {
                            Id = rideCarSeatComposition.Id,
                            CarId = rideResponse.Car.Id,
                            Passenger = mapper.Map<UserResponse>(rideCarSeatComposition.Passenger),
                            xCordinant = rideCarSeatComposition.CarSeatComposition.Seat.xCordinant,
                            yCordinant = rideCarSeatComposition.CarSeatComposition.Seat.yCordinant,
                            SeatRotate = rideCarSeatComposition.CarSeatComposition.SeatRotate,
                            SeatType = rideCarSeatComposition.SeatStatus,
                            Debt = rideCarSeatComposition.PaymentType == Domain.Enums.PaymentType.Cash ? 
                                ride.PricePerSeat : 0
                        };
                        rideResponse.Car.CarSeats.Add(carSeatCompositionResponse);
                    }

                    rideResponse.Car.CarSeats = rideResponse.Car.CarSeats.OrderBy(x =>
                        int.Parse(string.Concat(x.xCordinant, x.yCordinant)))
                        .ToList();
                }

                rideResponses.Add(rideResponse);
            }
            return rideResponses;
        }

        public static IEnumerable<PassengerToRideResponse> ToPassengerToRideResponse(this ICollection<PassengerToRideRequest> source, IMapper mapper)
        {
            foreach (var passengerToRideRequest in source)
            {
                var response = new PassengerToRideResponse
                {
                    Ride = RidesToRideResponses(new[] { passengerToRideRequest.RideCarSeatComposition.Ride }, mapper).First(),
                    RequestId = passengerToRideRequest.Id,
                    PassengerRequest = new CarSeatCompositionResponse
                    {
                        Passenger = mapper.Map<UserResponse>(passengerToRideRequest.User),
                        xCordinant = passengerToRideRequest.RideCarSeatComposition.CarSeatComposition.Seat.xCordinant,
                        yCordinant = passengerToRideRequest.RideCarSeatComposition.CarSeatComposition.Seat.yCordinant,
                        CarId = passengerToRideRequest.RideCarSeatComposition.CarSeatComposition.CarId,
                        SeatId = passengerToRideRequest.RideCarSeatComposition.CarSeatComposition.SeatId,
                        SeatRotate = passengerToRideRequest.RideCarSeatComposition.CarSeatComposition.SeatRotate,
                        SeatType = passengerToRideRequest.RideCarSeatComposition.CarSeatComposition.SeatType,
                        Id = passengerToRideRequest.RideCarSeatComposition.CarSeatComposition.Id,
                    }
                };

                response.Ride.Car.CarSeats = mapper.Map<ICollection<CarSeatCompositionResponse>>(passengerToRideRequest
                    .RideCarSeatComposition.CarSeatComposition.Car.CarSeatComposition);

                yield return response;
            }
        }
    }
}