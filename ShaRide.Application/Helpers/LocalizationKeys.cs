namespace ShaRide.Application.Helpers
{
    /// <summary>
    /// Keeps localization keys.
    /// </summary>
    public static class LocalizationKeys
    {
        /// <summary>
        /// Use Case: exception.
        /// General purpose of throwing not found exception.
        /// Has not founding exception parameter.
        /// </summary>
        public const string NOT_FOUND = "Exception.NotFound";

        /// <summary>
        /// User case: exception.
        /// Parent id cannot be null
        /// </summary>
        public const string PARENT_ID_NOT_PROVIDED = "Exception.ParentIdNotProvided";

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
        /// Use case : exception.
        /// Has phone parameter.
        /// </summary>
        public const string PHONE_ALREADY_CONFIRMED = "Exception.PhoneConfirmed";

        /// <summary>
        /// User case : exception.
        /// </summary>
        public const string LOCATION_NOT_FOUND = "Exception.LocationNotFound";
    }
}