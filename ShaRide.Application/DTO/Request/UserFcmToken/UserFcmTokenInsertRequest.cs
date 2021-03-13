namespace ShaRide.Application.DTO.Request.UserFcmToken
{
    public class UserFcmTokenInsertRequest
    {
        public int UserId { get; set; }
        public string Token { get; set; }
    }
}