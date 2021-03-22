namespace ShaRide.Application.DTO.Request.UserFcmToken
{
    public class UserFcmTokenDeleteRequest
    {
        public int UserId { get; set; }
        public string Token { get; set; }
    }
}