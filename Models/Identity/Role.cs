namespace Persistence.Models.Identity
{
    /// <summary>
    /// Represents a role in the application.
    /// This class is used to store role data in the database.
    /// </summary>
    public class Role : AbstractBaseModel
    {
        /// <summary>
        /// The name of the role.
        /// </summary>
        public string RoleName { get; set; } = string.Empty;
        /// <summary>
        /// A description of the role.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// An identifier for the role separate from the id. Used in case
        /// roles are removed and added. This number must be unique.
        /// </summary>
        public int RoleNumber { get; set; } = -1;
    }
}