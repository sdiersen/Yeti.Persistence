namespace Persistence.Migrations.Constants
{
    /// <summary>
    /// Column constant strings for UserData table
    /// </summary>
    public class DbUserDataTable : DbCommonColumns
    {
        /// <summary>
        /// The UserName column name.
        /// </summary>
        public const string USERNAME = "UserName";
        /// <summary>
        /// The Email column name.
        /// </summary>
        public const string EMAIL = "Email";
        /// <summary>
        /// The Password column name.
        /// </summary>
        public const string PASSWORD = "Password";
        /// <summary>
        /// The Salt column name.
        /// </summary>
        public const string LAST_LOGIN = "LastLogin";


    }
}