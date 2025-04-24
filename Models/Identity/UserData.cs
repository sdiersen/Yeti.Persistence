namespace Persistence.Models.Identity
{
    /// <summary>
    /// Represents a user in the application.
    /// This class is used to store user data in the database.
    /// </summary>
    public class UserData : AbstractBaseModel
    {
        /// <summary>
        /// The first name of the user.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// The last name of the user.
        /// </summary>
        public string LastName { get; set; } = string.Empty;
        /// <summary>
        /// The date of birth of the user.
        /// </summary>
        public DateOnly? DateOfBirth { get; set; } = null; // Nullable DateTime to allow for no date of birth

        /// <summary>
        /// The default user for the application.
        /// </summary>
        /// <returns>A UserData object with Username, Email, and Password set to string.Empty, LastLogin set to DateTime.Now, and Id set to -1.</returns>
        static public UserData DefaultUser()
        {
            return new UserData { Id = -1 };
        }
    }
}