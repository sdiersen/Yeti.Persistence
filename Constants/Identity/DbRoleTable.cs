namespace Persistence.Migrations.Constants
{
    /// <summary>
    /// This class contains the constants for the Role table
    /// </summary>
    public class DbRoleTable : DbCommonColumns
    {
        /// <summary>
        /// Role name of the user
        /// </summary>
        public const string ROLE_NAME = "RoleName";
        /// <summary>
        /// Description of the role
        /// </summary>
        public const string DESCRIPTION = "Description";
        /// <summary>
        /// An identifier for the role seperate from the id. Used in case
        /// roles are removed and added. This number must be unique.
        /// </summary>
        public const string ROLE_NUMBER = "RoleNumber";
    }
}