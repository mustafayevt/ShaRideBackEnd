using System.Linq;
using AutoMapper;
using ShaRide.Application.DTO.Request.Account;
using ShaRide.Application.DTO.Request.BanType;
using ShaRide.Application.DTO.Request.Car;
using ShaRide.Application.DTO.Request.CarBrand;
using ShaRide.Application.DTO.Request.CarModel;
using ShaRide.Application.DTO.Request.Common;
using ShaRide.Application.DTO.Request.Location;
using ShaRide.Application.DTO.Request.Restriction;
using ShaRide.Application.DTO.Request.Ride;
using ShaRide.Application.DTO.Response.BanType;
using ShaRide.Application.DTO.Response.Car;
using ShaRide.Application.DTO.Response.CarBrand;
using ShaRide.Application.DTO.Response.CarModel;
using ShaRide.Application.DTO.Response.Location;
using ShaRide.Application.DTO.Response.Restriction;
using ShaRide.Application.DTO.Response.Ride;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region UserPhone

            CreateMap<UserPhone, PhoneRequest>()
                .ForMember(x => x.Number, opt => opt.MapFrom(y => y.Number))
                .ForMember(x => x.IsMain, opt => opt.MapFrom(y => y.IsMain))
                .ReverseMap();

            #endregion

            #region ApplicationUser

            CreateMap<User, RegisterRequest>()
                .ForMember(x => x.FirstName, opt => opt.MapFrom(y => y.Name))
                .ForMember(x => x.LastName, opt => opt.MapFrom(y => y.Surname))
                .ForMember(x => x.Phones, opt => opt.MapFrom(y => y.Phones))
                .ForMember(x => x.Attachment, opt => opt.Ignore())
                .ReverseMap();

            #endregion

            #region Location

            //Location Points
            CreateMap<LocationPoint, InsertLocationPointRequest>().ReverseMap();
            CreateMap<LocationPoint, UpdateLocationPointRequest>().ReverseMap();
            CreateMap<LocationPoint, LocationPointResponse>().ReverseMap();

            CreateMap<Location, InsertLocationRequest>().ReverseMap();
            CreateMap<Location, UpdateLocationRequest>().ReverseMap();
            CreateMap<Location, LocationResponse>().ReverseMap();

            #endregion

            #region Restriction

            CreateMap<Restriction, InsertRestrictionRequest>().ReverseMap();
            CreateMap<Restriction, UpdateRestrictionRequest>().ReverseMap();
            CreateMap<Restriction, RestrictionResponse>().ReverseMap();

            #endregion

            #region BanType

            CreateMap<BanType, InsertBanTypeRequest>().ReverseMap();
            CreateMap<BanType, UpdateBanTypeRequest>().ReverseMap();
            CreateMap<BanType, BanTypeResponse>().ReverseMap();

            #endregion

            #region CarBrand

            CreateMap<CarBrand, InsertCarBrandRequest>().ReverseMap();
            CreateMap<CarBrand, UpdateCarBrandRequest>().ReverseMap();
            CreateMap<CarBrand, CarBrandResponse>().ReverseMap();

            #endregion

            #region CarModel

            CreateMap<CarModel, InsertCarModelRequest>().ReverseMap();
            CreateMap<CarModel, UpdateCarModelRequest>().ReverseMap();
            CreateMap<CarModel, CarModelResponse>().ReverseMap();

            #endregion

            #region Ride

            CreateMap<RideLocationPointComposition, RideLocationPointRequest>()
                .ForMember(x => x.LocationPointId, opt => opt.MapFrom(y => y.LocationPointId))
                .ForMember(x => x.LocationPointType, opt => opt.MapFrom(y => y.LocationPointType))
                .ReverseMap();

            CreateMap<RestrictionRideComposition, RideRestrictionRequest>()
                .ForMember(x => x.IsPossible, opt => opt.MapFrom(y => y.IsPossible))
                .ForMember(x => x.RestrictionId, opt => opt.MapFrom(y => y.RestrictionId))
                .ReverseMap();

            CreateMap<Ride, InsertRideRequest>()
                .ForMember(x => x.DriverId, opt => opt.MapFrom(y => y.DriverId))
                .ForMember(x => x.StartDate, opt => opt.MapFrom(y => y.StartDate))
                .ForMember(x => x.PricePerSeat, opt => opt.MapFrom(y => y.PricePerSeat))
                .ForMember(x => x.Note, opt => opt.MapFrom(y => y.Note))
                .ForMember(x => x.RideState, opt => opt.MapFrom(y => y.RideState))
                .ForMember(x => x.RideLocationPoints, opt => opt.MapFrom(y => y.RideLocationPointComposition))
                .ForMember(x => x.RideRestrictions, opt => opt.MapFrom(y => y.RestrictionRideComposition))
                .ForMember(x => x.RideSeatRequests, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Ride, RideResponse>()
                .ForMember(x=>x.Id,opt=>opt.MapFrom(y=>y.Id))
                .ReverseMap();
            
            #endregion

            #region Car

            CreateMap<CarImage, AttachmentRequest>()
                .ForMember(x => x.Content, opt => opt.MapFrom(y => y.Image.ToList()))
                .ForMember(x => x.Extension, opt => opt.MapFrom(y => y.Extension));
            CreateMap<AttachmentRequest, CarImage>()
                .ForMember(x => x.Image, opt => opt.MapFrom(y => y.Content.ToArray()))
                .ForMember(x => x.Extension, opt => opt.MapFrom(y => y.Extension));

            CreateMap<CarSeatComposition, InsertCarSeatCompositionRequest>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.SeatId))
                .ReverseMap();

            CreateMap<CarSeatComposition, CarSeatCompositionResponse>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.SeatId))
                .ForMember(x => x.xCordinant, opt => opt.MapFrom(y => y.Seat.xCordinant))
                .ForMember(x => x.yCordinant, opt => opt.MapFrom(y => y.Seat.yCordinant))
                .ForMember(x => x.SeatType, opt => opt.MapFrom(y => y.SeatType))
                .ReverseMap();
            
            CreateMap<Car, InsertCarRequest>()
                .ForMember(x => x.ModelId, opt => opt.MapFrom(y => y.CarModelId))
                .ForMember(x => x.RegisterNumber, opt => opt.MapFrom(y => y.RegisterNumber))
                .ForMember(x => x.CarImages, opt => opt.MapFrom(y => y.CarImages))
                .ForMember(x => x.CarSeats, opt => opt.MapFrom(y => y.CarSeatComposition))
                .ReverseMap();
            
            CreateMap<Car, CarResponse>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.CarModelId))
                .ForMember(x => x.RegisterNumber, opt => opt.MapFrom(y => y.RegisterNumber))
                .ForMember(x => x.CarImageIds, opt => opt.MapFrom(x=>x.CarImages.Select(x=>x.Id)))
                .ForMember(x => x.CarSeats, opt => opt.MapFrom(y => y.CarSeatComposition))
                .ForMember(x => x.Model, opt => opt.MapFrom(y => y.CarModel))
                .ReverseMap();

            #endregion
        }
    }
}