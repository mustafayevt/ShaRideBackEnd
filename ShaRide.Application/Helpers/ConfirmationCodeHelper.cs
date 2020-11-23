using System;

namespace ShaRide.Application.Helpers
{
    public static class ConfirmationCodeHelper
    {
        public static string GenerateConfirmationCode()
        {
            Random random = new Random();
            int result = random.Next(1000, 9999);
            return result.ToString();
        }
    }
}