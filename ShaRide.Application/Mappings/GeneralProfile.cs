using System.Linq;
using AutoMapper;
using ShaRide.Application.DTO.Request;
using ShaRide.Application.DTO.Request.Account;
using ShaRide.Application.DTO.Request.BanType;
using ShaRide.Application.DTO.Request.CarBrand;
using ShaRide.Application.DTO.Request.CarModel;
using ShaRide.Application.DTO.Request.Location;
using ShaRide.Application.DTO.Request.Restriction;
using ShaRide.Application.DTO.Response;
using ShaRide.Application.DTO.Response.BanType;
using ShaRide.Application.DTO.Response.CarBrand;
using ShaRide.Application.DTO.Response.CarModel;
using ShaRide.Application.DTO.Response.Location;
using ShaRide.Application.DTO.Response.Restriction;
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
        }
    }
}