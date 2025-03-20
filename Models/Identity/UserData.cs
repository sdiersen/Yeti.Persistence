using Microsoft.AspNetCore.DataProtection;

using Persistence.Helpers;

namespace Persistence.Models.Identity
{
    /// <summary>
    /// Represents a user in the application.
    /// This class is used to store user data in the database.
    /// </summary>
    public class UserData : AbstractBaseModel
    {
        /// <summary>
        /// The username of the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// The email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// The password of the user.
        /// </summary>
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// The date and time the user last logged in.
        /// </summary>
        public DateTime LastLogin { get; set; } = DateTime.Now; // program generated only

        /// <summary>
        /// The default user for the application.
        /// </summary>
        /// <returns>A UserData object with Username, Email, and Password set to string.Empty, LastLogin set to DateTime.Now, and Id set to -1.</returns>
        static public UserData DefaultUser()
        {
            return new UserData { Id = -1 };
        }
    }

    /// <summary>
    /// Extension methods for the UserData class.
    /// These methods are used to protect and unprotect the data in the UserData object.
    /// </summary>
    public static class UserDataExtensions
    {
        /// <summary>
        /// Protects the data in the UserData object. This method uses IDataProtectionProvider to protect the data.
        /// Only the Username, Email, and Password properties are protected.
        /// </summary>
        /// <param name="userData">this UserData object</param>
        /// <returns>A UserData object so that this method can be chained.</returns>
        public static UserData ProtectData(this UserData userData)
        {
            var dataProtector = DataProtectionConfig.CreateProtector("UserData");
            userData.Username = dataProtector.Protect(userData.Username);
            userData.Email = dataProtector.Protect(userData.Email);
            userData.Password = dataProtector.Protect(userData.Password);

            return userData;
        }

        /// <summary>
        /// Unprotects the data in the UserData object. This method uses IDataProtectionProvider to unprotect the data.
        /// Only the Username, Email, and Password properties are unprotected.
        /// </summary>
        /// <param name="userData">this UserData object</param>
        /// <returns>A UserData object so that this method can be chained </returns>
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