using Microsoft.AspNetCore.DataProtection;
using Persistence.Helpers;

namespace Persistence.Models.Entry
{
    public class Item : AbstractBaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0.0m;
    }

    public static class ItemExtensions
    {
        // TODO protection in general will run into issues when not using bytes for the data.
        // The database is setup for all types of data, so encryption will be a problem for later.
        public static Item ProtectData(this Item item)
        {
            var dataProtector = DataProtectionConfig.CreateProtector("Item");
            item.Name = dataProtector.Protect(item.Name);
            item.Description = dataProtector.Protect(item.Description);

            return item;
        }

        public static Item UnprotectData(this Item item)
        {
            var dataProtector = DataProtectionConfig.CreateProtector("Item");
            item.Name = dataProtector.Unprotect(item.Name);
            item.Description = dataProtector.Unprotect(item.Description);

            return item;
        }
    }
}