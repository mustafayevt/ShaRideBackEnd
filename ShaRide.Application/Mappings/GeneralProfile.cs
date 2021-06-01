using System.Linq;
using AutoMapper;
using ShaRide.Application.DTO.Request.Account;
using ShaRide.Application.DTO.Request.BanType;
using ShaRide.Application.DTO.Request.Car;
using ShaRide.Application.DTO.Request.CarBrand;
using ShaRide.Application.DTO.Request.CarModel;
using ShaRide.Application.DTO.Request.Common;
using ShaRide.Application.DTO.Request.Feedback;
using ShaRide.Application.DTO.Request.Invoice;
using ShaRide.Application.DTO.Request.Location;
using ShaRide.Application.DTO.Request.Message;
using ShaRide.Application.DTO.Request.Restriction;
using ShaRide.Application.DTO.Request.Ride;
using ShaRide.Application.DTO.Request.UserFcmToken;
using ShaRide.Application.DTO.Request.UserRating;
using ShaRide.Application.DTO.Response.Account;
using ShaRide.Application.DTO.Response.BanType;
using ShaRide.Application.DTO.Response.Car;
using ShaRide.Application.DTO.Response.CarBrand;
using ShaRide.Application.DTO.Response.CarModel;
using ShaRide.Application.DTO.Response.Feedback;
using ShaRide.Application.DTO.Response.Invoice;
using ShaRide.Application.DTO.Response.Location;
using ShaRide.Application.DTO.Response.Restriction;
using ShaRide.Application.DTO.Response.Ride;
using ShaRide.Application.DTO.Response.UserFcmToken;
using ShaRide.Application.DTO.Response.UserPhone;
using ShaRide.Application.DTO.Response.UserRating;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Mappings
{
    public class 
        GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region User

            CreateMap<User, UserResponse>();

            #endregion
            
            #region UserPhone

            CreateMap<UserPhone, PhoneRequest>()
                .ForMember(x => x.Number, opt => opt.MapFrom(y => y.Number))
                .ForMember(x => x.IsMain, opt => opt.MapFrom(y => y.IsMain))
                .ReverseMap();

            CreateMap<UserPhone, UserPhoneResponse>();

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
            CreateMap<LocationPoint, LocationPointResponse>()
                .ForMember(x=>x.LocationName, opt=>opt.MapFrom(y=>y.Location.Name))
                .ReverseMap();

            CreateMap<Location, InsertLocationRequest>().ReverseMap();
            CreateMap<Location, UpdateLocationRequest>().ReverseMap();
            CreateMap<Location, LocationResponse>().ReverseMap();

            CreateMap<RideLocationPointComposition, RideLocationPointCompositionResponse>();

            #endregion

            #region Restriction

            CreateMap<Restriction, InsertRestrictionRequest>().ReverseMap();
            CreateMap<Restriction, UpdateRestrictionRequest>().ReverseMap();
            CreateMap<Restriction, RestrictionResponse>().ReverseMap();
            CreateMap<RideRestrictionRequest, RestrictionResponse>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.RestrictionId))
                .ReverseMap();

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

            CreateMap<RideCarSeatComposition, RideSeatRequest>()
                .ForMember(x => x.CarSeatCompositionId, opt => opt.MapFrom(y => y.CarSeatCompositionId))
                .ForMember(x => x.SeatStatus, opt => opt.MapFrom(y => y.SeatStatus))
                .ReverseMap();

            CreateMap<Ride, InsertRideRequest>()
                .ForMember(x => x.DriverId, opt => opt.MapFrom(y => y.DriverId))
                .ForMember(x => x.StartDate, opt => opt.MapFrom(y => y.StartDate))
                .ForMember(x => x.PricePerSeat, opt => opt.MapFrom(y => y.PricePerSeat))
                .ForMember(x => x.Note, opt => opt.MapFrom(y => y.Note))
                .ForMember(x => x.RideState, opt => opt.MapFrom(y => y.RideState))
                // .ForMember(x => x.RideLocationPoints, opt => opt.MapFrom(y => y.RideLocationPointComposition))
                .ForMember(x => x.RideRestrictions, opt => opt.MapFrom(y => y.RestrictionRideComposition))
                .ForMember(x => x.RideSeatRequests, opt => opt.MapFrom(y=>y.RideCarSeatComposition))
                .ReverseMap();

            CreateMap<Ride, RideResponse>()
                .ForMember(x => x.Restrictions, opt => opt.MapFrom(y => y.RestrictionRideComposition.Select(y=>new RestrictionResponse
                {
                    Id = y.Restriction.Id,
                    Title = y.Restriction.Title,
                    AssetPath = y.Restriction.AssetPath,
                    IsPossible = y.IsPossible
                })))
                .ForMember(x => x.LocationPoints, opt => opt.MapFrom(y => y.RideLocationPointComposition))
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
                .ForMember(x => x.SeatId, opt => opt.MapFrom(y => y.SeatId))
                .ReverseMap();

            CreateMap<CarSeatComposition, CarSeatCompositionResponse>()
                .ForMember(x => x.xCordinant, opt => opt.MapFrom(y => y.Seat.xCordinant))
                .ForMember(x => x.yCordinant, opt => opt.MapFrom(y => y.Seat.yCordinant))
                .ForMember(x => x.SeatType, opt => opt.MapFrom(y => y.SeatType))
                .ReverseMap();
            
            CreateMap<Car, InsertCarRequest>()
                .ForMember(x => x.ModelId, opt => opt.MapFrom(y => y.CarModelId))
                .ForMember(x => x.BanTypeId, opt => opt.MapFrom(y => y.BanTypeId))
                .ForMember(x => x.RegisterNumber, opt => opt.MapFrom(y => y.RegisterNumber))
                .ForMember(x => x.CarImages, opt => opt.MapFrom(y => y.CarImages))
                .ForMember(x => x.CarSeats, opt => opt.MapFrom(y => y.CarSeatComposition))
                .ReverseMap();
            
            CreateMap<Car, CarResponse>()
                .ForMember(x => x.RegisterNumber, opt => opt.MapFrom(y => y.RegisterNumber))
                .ForMember(x => x.CarImageIds, opt => opt.MapFrom(x=>x.CarImages.Select(carImage=>carImage.Id)))
                .ForMember(x => x.CarSeats, opt => opt.MapFrom(y => y.CarSeatComposition))
                .ForMember(x => x.Model, opt => opt.MapFrom(y => y.CarModel))
                .ForMember(x => x.BanType, opt => opt.MapFrom(y => y.BanType))
                .ReverseMap();
            
            CreateMap<Car, RideCarResponse>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.CarModelId))
                .ForMember(x => x.RegisterNumber, opt => opt.MapFrom(y => y.RegisterNumber))
                .ForMember(x => x.CarImageIds, opt => opt.MapFrom(x=>x.CarImages.Select(carImage=>carImage.Id)))
                .ForMember(x => x.CarSeats, opt => opt.MapFrom(y => y.CarSeatComposition))
                .ForMember(x => x.Model, opt => opt.MapFrom(y => y.CarModel))
                .ForMember(x => x.BanType, opt => opt.MapFrom(y => y.BanType))
                .ReverseMap();

            CreateMap<RideCarSeatComposition, RideCarSeatCompositionResponse>()
                .ForMember(x=>x.Car,opt=>opt.MapFrom(y=>y.CarSeatComposition.Car));

            #endregion

            #region Invoice

            CreateMap<Invoice, InvoiceResponse>()
                .ForMember(x=>x.UserId,opt=>opt.MapFrom(y=>y.User.UserUniqueKey))
                .ForMember(x=>x.Name,opt=>opt.MapFrom(y=>y.User.Name))
                .ForMember(x=>x.Surname,opt=>opt.MapFrom(y=>y.User.Surname))
                .ReverseMap();
            CreateMap<Invoice, RegisterInvoiceRequest>()
                .ForMember(x => x.UserId, opt => opt.Ignore());
            CreateMap<RegisterInvoiceRequest, Invoice>()
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.UserId, opt => opt.Ignore());
            CreateMap<RegisterInvoiceRequest, InvoiceResponse>()
                .ReverseMap();

            #endregion

            #region Feedback

            CreateMap<InsertFeedbackRequest, Feedback>()
                .ReverseMap();

            CreateMap<FeedbackResponse, Feedback>()
                .ForMember(x=>x.CreatedByUser,opt=>opt.MapFrom(y=>y.UserResponse))
                .ReverseMap();

            #endregion

            #region UserRating

            CreateMap<UserRatingResponse, InsertUserRatingRequest>().ReverseMap();
            CreateMap<RatingRequest, RatingResponse>().ReverseMap();

            #endregion

            #region UserFcmToken

            CreateMap<UserFcmToken, UserFcmTokenInsertRequest>().ReverseMap();
            CreateMap<UserFcmToken, UserFcmTokenUpdateRequest>().ReverseMap();
            CreateMap<UserFcmToken, UserFcmTokenResponse>().ReverseMap();

            #endregion

            #region Message

            CreateMap<Message, InsertMessageRequest>().ReverseMap();

            #endregion
        }
    }
}