using Microsoft.AspNetCore.DataProtection;

using Persistence.Helpers;

namespace Persistence.Models.Entry
{
    /// <summary>
    /// Represents a category that an entry can be associated with.
    /// </summary>
    public class EntryCategory : AbstractBaseModel
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
        public static EntryCategory DefaultCategory()
        {
            return new EntryCategory { Id = -1 };
        }
    }

    /// <summary>
    /// Extension methods for the EntryCategory class.
    /// </summary>
    public static class EntryClassExtensions
    {
        /// <summary>
        /// Protects the data in the EntryCategory object. This method uses IDataProtectionProvider to protect the data.
        /// Only the Name and Description properties are protected.
        /// </summary>
        /// <param name="entryCategory">this EntryCattegory object</param>
        /// <returns>an EntryCategory object so that this method can be chained</returns>
        public static EntryCategory ProtectData(this EntryCategory entryCategory)
        {
            var dataProtector = DataProtectionConfig.CreateProtector("EntryCategory");
            entryCategory.Name = dataProtector.Protect(entryCategory.Name);
            entryCategory.Description = dataProtector.Protect(entryCategory.Description);

            return entryCategory;

        }
        /// <summary>
        /// Unprotects the data in the EntryCategory object. This method uses IDataProtectionProvider to unprotect the data.
        /// Only the Name and Description properties are unprotected.
        /// </summary>
        /// <param name="entryCategory">this EntryCategory object</param>
        /// <returns>an EntryCategory object so that this method can be chained</returns>
        public static EntryCategory UnprotectData(this EntryCategory entryCategory)
        {
            var dataProtector = DataProtectionConfig.CreateProtector("EntryCategory");
            entryCategory.Name = dataProtector.Unprotect(entryCategory.Name);
            entryCategory.Description = dataProtector.Unprotect(entryCategory.Description);

            return entryCategory;
        }
    }
}