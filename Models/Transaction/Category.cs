using Microsoft.AspNetCore.DataProtection;

using Persistence.Helpers;

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

    /// <summary>
    /// Extension methods for the EntryCategory class.
    /// </summary>
    public static class CategoryExtensions
    {
        /// <summary>
        /// Protects the data in the Category object. This method uses IDataProtectionProvider to protect the data.
        /// Only the Name and Description properties are protected.
        /// </summary>
        /// <param name="category">this Category object</param>
        /// <returns>an EntryCategory object so that this method can be chained</returns>
        public static Category ProtectData(this Category category)
        {
            var dataProtector = DataProtectionConfig.CreateProtector("Category");
            category.Name = dataProtector.Protect(category.Name);
            category.Description = dataProtector.Protect(category.Description);

            return category;

        }
        /// <summary>
        /// Unprotects the data in the Category object. This method uses IDataProtectionProvider to unprotect the data.
        /// Only the Name and Description properties are unprotected.
        /// </summary>
        /// <param name="Category">this Category object</param>
        /// <returns>an EntryCategory object so that this method can be chained</returns>
        public static Category UnprotectData(this Category Category)
        {
            var dataProtector = DataProtectionConfig.CreateProtector("Category");
            Category.Name = dataProtector.Unprotect(Category.Name);
            Category.Description = dataProtector.Unprotect(Category.Description);

            return Category;
        }
    }
}