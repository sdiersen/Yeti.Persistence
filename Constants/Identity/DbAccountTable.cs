namespace Persistence.Migrations.Constants
{
    /// <summary>
    /// Column constant strings for Account table
    /// </summary>
    public class DbAccountTable : DbCommonColumns
    {
        /// <summary>
        /// UserName of the account
        /// </summary>
        public const string USERNAME = "UserName";
        /// <summary>
        /// Email of the account
        /// </summary>
        public const string PASSWORD = "Password";
        /// <summary>
        /// Date of last login
        /// </summary>
        public const string LAST_LOGIN = "LastLogin";
        /// <summary>
        /// Indicates if the account is active
        /// </summary>
        public const string IS_ACTIVE = "IsActive";
        /// <summary>
        /// Indicates if the account is locked
        /// </summary>
        public const string IS_LOCKED = "IsLocked";
    }
}