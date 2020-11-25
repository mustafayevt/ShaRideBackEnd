using System.Linq;
using AutoMapper;
using ShaRide.Application.DTO.Request;
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
        }
    }
}