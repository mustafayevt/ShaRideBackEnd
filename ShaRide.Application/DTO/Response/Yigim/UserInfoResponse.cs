namespace ShaRide.Application.DTO.Response.Yigim
{
    public class UserInfoResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public decimal Balance { get; set; }

        public UserInfoResponse(string name, string surname, decimal balance)
        {
            Name = name;
            Surname = surname;
            Balance = balance;
        }
    }
}