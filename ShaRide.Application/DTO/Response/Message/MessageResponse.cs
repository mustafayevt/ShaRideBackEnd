using ShaRide.Application.DTO.Response.Account;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Response.Message
{
    public class MessageResponse
    {
        public int MessageId { get; set; }
        public int RideId { get; set; }
        public string Content { get; set; }
        
        public MessageType MessageType { get; set; }
        
        public MessageSenderType SenderType { get; set; }

        public int SenderId { get; set; }

        public string SenderFullname { get; set; }

        public short SenderRating { get; set; }

        public string MessageDatetime { get; set; }
    }
}