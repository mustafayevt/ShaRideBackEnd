using System.Collections.Generic;
using ShaRide.Application.DTO.Response.Account;

namespace ShaRide.Application.DTO.Response.Message
{
    public class GetMessageGroupVm
    {
        public int RideId { get; set; }
        public int DriverId { get; set; }
        public string RideStartPoint { get; set; }
        public string RideEndPoint { get; set; }
        public string RideDateTime { get; set; }
        public ICollection<MessageResponse> Messages { get; set; }
        public ICollection<RideParticipantVm> Participants { get; set; }
    }
}