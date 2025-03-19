using Microsoft.AspNetCore.DataProtection;
using Persistence.Helpers;

namespace Persistence.Models.Entry
{
    // EntryCategory is a category for an expense entry. 
    // Both prepopulated and user-created categories are stored in the Category table.
    public class EntryCategory : AbstractBaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public static EntryCategory DefaultCategory()
        {
            return new EntryCategory { Id = -1 };
        }
    }

    public static class EntryClassExtensions
    {
        public static EntryCategory ProtectData(this EntryCategory entryCategory)
        {
            var dataProtector = DataProtectionConfig.CreateProtector("EntryCategory");
            entryCategory.Name = dataProtector.Protect(entryCategory.Name);
            entryCategory.Description = dataProtector.Protect(entryCategory.Description);

            return entryCategory;

        }
        public static EntryCategory UnprotectData(this EntryCategory entryCategory)
        {
            var dataProtector = DataProtectionConfig.CreateProtector("EntryCategory");
            entryCategory.Name = dataProtector.Unprotect(entryCategory.Name);
            entryCategory.Description = dataProtector.Unprotect(entryCategory.Description);

            return entryCategory;
        }
    }
}