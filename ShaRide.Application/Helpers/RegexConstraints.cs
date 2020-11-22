namespace ShaRide.Application.Helpers
{
    public static class RegexConstraints
    {
        public static string HasUpperCases = @"[A-Z]+";
        public static string HasLowerCases = @"[a-z]+";
        public static string HasNumbers = @"[0-9]+";
        public static string HasCharacters = @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]";
        public static string Length = @".{6,100}";
        public static string NameSurname = @"^[A-Za-z]+$";
        public static string Phone = @"^(\+[0-9]{12})$";
    }
}