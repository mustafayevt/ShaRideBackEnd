using ShaRide.Domain.Entities;

namespace ShaRide.Application.DTO.Response.Phone
{
    public class UserPhoneResponse
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public bool IsMain { get; set; }
        public bool IsConfirmed { get; set; }
    }
}