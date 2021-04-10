using System.Collections.Generic;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Response.Message
{
    public class RateUserNotificationVm : IModel
    {
        public int RideId { get; set; }
        public IList<RateUserNotificationDetailVm> Participants { get; set; }

        public class RateUserNotificationDetailVm
        {
            public int UserId { get; set; }
            public string UserFullname { get; set; }
            public MessageSenderType Type { get; set; }
        }
    }
}