namespace ShaRide.Application.DTO.Request.UserFcmToken
{
    public class UserFcmTokenUpdateRequest
    {
        public string OldToken { get; set; }
        public string NewToken { get; set; }
        public int UserId { get; set; }
    }
}