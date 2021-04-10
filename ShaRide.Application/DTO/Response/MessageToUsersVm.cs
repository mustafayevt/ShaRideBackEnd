using System;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Response
{
    public class MessageToUsersVm
    {
        public int MessageId { get; set; }
        public int RideId { get; set; }
        public int SenderId { get; set; }
        public string SenderFullname { get; set; }
        public short SenderUserRating { get; set; }
        public string Content { get; set; }
        public MessageType MessageType { get; set; }
        public MessageSenderType SenderType { get; set; }
        public DateTime DateTime { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public DateTime RideStartDate { get; set; }

        public MessageToUsersVm(int messageId, string senderFullname, short senderUserRating, string content, MessageType messageType, MessageSenderType senderType, DateTime dateTime, string startPoint, string endPoint, DateTime rideStartDate, int rideId, int senderId)
        {
            MessageId = messageId;
            SenderFullname = senderFullname;
            SenderUserRating = senderUserRating;
            Content = content;
            MessageType = messageType;
            SenderType = senderType;
            DateTime = dateTime;
            StartPoint = startPoint;
            EndPoint = endPoint;
            RideStartDate = rideStartDate;
            RideId = rideId;
            SenderId = senderId;
        }
    }
}