namespace Persistence.Models.Transaction
{
    /// <summary>
    /// Represents a category that an entry can be associated with.
    /// </summary>
    public class Category : AbstractBaseModel
    {
        /// <summary>
        /// The name of the category.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// A description of the category.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// The default category for an entry.
        /// </summary>
        /// <returns>A new EntryCategory with Name = string.Empty, Description = string.Empty and Id = -1 </returns>
        public static Category DefaultCategory()
        {
            return new Category { Id = -1 };
        }
    }
}