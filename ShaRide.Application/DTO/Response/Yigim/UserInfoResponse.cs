namespace ShaRide.Application.DTO.Response.Yigim
{
    public class UserInfoResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public decimal Debt { get; set; }

        public UserInfoResponse(string name, string surname, decimal debt)
        {
            Name = name;
            Surname = surname;
            Debt = debt;
        }
    }
}