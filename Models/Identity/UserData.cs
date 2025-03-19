using Microsoft.AspNetCore.DataProtection;
using Persistence.Helpers;

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

    public static class UserDataExtensions
    {
        public static UserData ProtectData(this UserData userData)
        {
            var dataProtector = DataProtectionConfig.CreateProtector("UserData");
            userData.Username = dataProtector.Protect(userData.Username);
            userData.Email = dataProtector.Protect(userData.Email);
            userData.Password = dataProtector.Protect(userData.Password);

            return userData;
        }

        public static UserData UnprotectData(this UserData userData)
        {
            var dataProtector = DataProtectionConfig.CreateProtector("UserData");
            userData.Username = dataProtector.Unprotect(userData.Username);
            userData.Email = dataProtector.Unprotect(userData.Email);
            userData.Password = dataProtector.Unprotect(userData.Password);

            return userData;
        }
    }
}