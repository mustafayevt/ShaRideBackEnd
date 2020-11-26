namespace ShaRide.Application.DTO.Response.Account
{
    public class AuthenticationResponse
    {
        public int Id { get; set; }
        public bool IsVerified { get; set; }
        public string JWToken { get; set; }
    }
}
