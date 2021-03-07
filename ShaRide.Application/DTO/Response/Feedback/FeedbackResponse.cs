using ShaRide.Application.DTO.Response.Account;

namespace ShaRide.Application.DTO.Response.Feedback
{
    public class FeedbackResponse
    {
        public int Id { get; set; }
        public UserResponse UserResponse { get; set; }
        public string Content { get; set; }
    }
}