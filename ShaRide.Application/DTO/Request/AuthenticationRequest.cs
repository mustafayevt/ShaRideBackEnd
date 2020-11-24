namespace ShaRide.Application.DTO.Request
{
    /// <summary>
    /// Authentication request model.
    /// </summary>
    public class AuthenticationRequest
    {
        /// <summary>
        /// Phone number.
        /// </summary>
        public string Phone { get; set; }
        
        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; set; }
    }
}
