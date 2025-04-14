namespace Persistence.Models.Identity
{
    /// <summary>
    /// This is a junction table between Account and Role.
    /// This table is used to associate accounts with their roles.
    /// </summary>
    public class AccountRole : AbstractBaseModel
    {
        /// <summary>
        /// The foreign key to the Account table.
        /// </summary>
        public int AccountId { get; set; } = -1;
        /// <summary>
        /// The foreign key to the Role table.
        /// </summary>
        public int RoleId { get; set; } = -1;
    }
}