namespace Persistence.Migrations.Constants
{
    /// <summary>
    /// Constant column names for the AccountRole table.
    /// </summary>
    public class DbAccountRoleTable : DbCommonColumns
    {
        /// <summary>
        /// This is a foreign key to the Account table.
        /// </summary>
        public const string ACCOUNT_ID = "AccountId";
        /// <summary>
        /// This is a foreign key to the Role table.
        /// </summary>
        public const string ROLE_ID = "RoleId";
    }
}