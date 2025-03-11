namespace Persistence.Models.Identity
{
    public class UserData : AbstractBaseModel
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime LastLogin { get; set; } = DateTime.Now; // program generated only

        static public UserData DefaultUser()
        {
            return new UserData { Id = -1 };
        }
    }
}