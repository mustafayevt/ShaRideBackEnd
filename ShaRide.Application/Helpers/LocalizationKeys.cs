namespace ShaRide.Application.Helpers
{
    /// <summary>
    /// Keeps localization keys.
    /// </summary>
    public static class LocalizationKeys
    {
        /// <summary>
        /// Use case : exception.
        /// Has phone parameter.
        /// </summary>
        public const string INVALID_CREDENTIALS = "Exception.InvalidCredentials";

        /// <summary>
        /// Use case : exception.
        /// Has phone parameter.
        /// </summary>
        public const string PHONE_ALREADY_TAKEN = "Exception.PhoneTaken";

        /// <summary>
        /// User case : exception.
        /// Has phone parameter.
        /// </summary>
        public const string PHONE_ALREADY_CONFIRMED = "Exception.PhoneConfirmed";
    }
}