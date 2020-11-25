namespace ShaRide.Domain.Entities
{
    public class UserRoleComposition
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public UserRoleComposition(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public UserRoleComposition()
        {
            
        }
    }
}