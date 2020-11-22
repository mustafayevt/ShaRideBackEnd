using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShaRide.Application.DTOs.Account
{
    public class AuthenticationResponse
    {
        public int Id { get; set; }
        public bool IsVerified { get; set; }
        public string JWToken { get; set; }
    }
}
