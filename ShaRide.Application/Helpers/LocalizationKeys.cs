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
        /// Use case : exception.
        /// Has 'already existing field' parameter.
        /// </summary>
        public const string ALREADY_EXISTS = "Exception.AlreadyExists";

        /// <summary>
        /// Use case: exception.
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
        /// Use case : exception.
        /// Has phone parameter.
        /// </summary>
        public const string PHONE_NOT_FOUND = "Exception.PhoneNotFound";

        /// <summary>
        /// Use case : exception.
        /// </summary>
        public const string LOCATION_NOT_FOUND = "Exception.LocationNotFound";

        /// <summary>
        /// Use case : exception.
        /// </summary>
        public const string RESTRICTION_ALREADY_EXISTS = "Exception.RestrictionAlreadyExists";

        /// <summary>
        /// User case : exception.
        /// Has id parameter.
        /// </summary>
        public const string RESTRICTOIN_NOT_FOUND = "Exception.RestrictionNotFound";

        /// <summary>
        /// Use case : validation error.
        /// Has property name parameter.
        /// </summary>
        public const string REQUIRED = "Validation.Required";

        /// <summary>
        /// Use case: validation error.
        /// Has property name parameter.
        /// </summary>
        public const string EMAIL = "Validation.Email";

        /// <summary>
        /// Use case : validation error.
        /// Has property name parameter.
        /// </summary>
        public const string PHONE = "Validation.Phone";

        /// <summary>
        /// Use case: validation error.
        /// </summary>
        public const string PASSWORD = "Validation.Password";

        /// <summary>
        /// Use case : validation error.
        /// </summary>
        public const string RANGE_VALIDATION = "Validation.Range";

        /// <summary>
        /// Use case : sending confirmation code.
        /// </summary>
        public const string CONFIRMATION_SMS = "VerificationSms.Confirmation";

        /// <summary>
        /// Use case : exception.
        /// Has id parameter
        /// </summary>
        public const string RIDE_NOT_FOUND = "Exception.RideNotFound";

        /// <summary>
        /// Use case : exception.
        /// </summary>
        public const string USER_HAS_NOT_ACCESS_OPERATION = "Validation.UserHasNotAccessToOperation";

        /// <summary>
        /// Use case : validation.
        /// General not enough balance validation.
        /// </summary>
        public const string USER_DOES_NOT_HAVE_ENOUGH_BALANCE = "Validation.UserDoesNotHaveEnoughBalance";

        public const string RIDE_CANNOT_CANCELLED = "Validation.RideCannotCancelled";
    }
}