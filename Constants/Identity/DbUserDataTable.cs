namespace Persistence.Migrations.Constants
{
    /// <summary>
    /// Column constant strings for UserData table
    /// </summary>
    public class DbUserDataTable : DbCommonColumns
    {
        /// <summary>
        /// First name of the user
        /// </summary>
        public const string FIRST_NAME = "FirstName";
        /// <summary>
        /// Last name of the user
        /// </summary>
        public const string LAST_NAME = "LastName";
        /// <summary>
        /// User's date of birth
        /// </summary>
        public const string DATE_OF_BIRTH = "DateOfBirth";
    }
}