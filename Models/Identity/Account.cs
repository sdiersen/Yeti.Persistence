namespace Persistence.Models.Identity
{
    /// <summary>
    /// Represents an account in the application.
    /// </summary>
    public class Account : AbstractBaseModel
    {
        /// <summary>
        /// The username of the account. Must be unique
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// The email of the account. Must be unique
        /// </summary>
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// The date of the last login. This is used to determine if the user is active or not.
        /// </summary>
        public DateTime LastLogin { get; set; } = DateTime.Now;
        /// <summary>
        /// This indicates if the account is active or not. This is used to determine if the user can log in or not.
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// This indicates if the account is locked or not. This is used to determine if the user can log in or not.
        /// </summary>
        public bool IsLocked { get; set; } = false;
    }
}