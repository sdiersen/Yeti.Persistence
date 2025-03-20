using Microsoft.AspNetCore.DataProtection;

using Persistence.Helpers;

namespace Persistence.Models.Entry
{
    /// <summary>
    /// Represents a transaction that can be either an expense or an income.
    /// </summary>
    public class Item : AbstractBaseModel
    {
        /// <summary>
        /// The name of the transaction.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// The description of the transaction.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// The amount of the transaction.
        /// an amount greater than or equal to 0 is an income, otherwise it is an expense.
        /// </summary>
        public decimal Amount { get; set; } = 0.0m;
    }

    /// <summary>
    /// Extension methods for the Item class.
    /// </summary>
    public static class ItemExtensions
    {
        // TODO protection in general will run into issues when not using bytes for the data.
        // The database is setup for all types of data, so encryption will be a problem for later.

        /// <summary>
        /// Protects the data in the Item object. This method uses IDataProtectionProvider to protect the data.
        /// Only the Name and Description properties are protected.
        /// amount protection is a problem for later.
        /// </summary>
        /// <param name="item">this Item object</param>
        /// <returns>an Item object so that this method can be chained</returns>
        public static Item ProtectData(this Item item)
        {
            var dataProtector = DataProtectionConfig.CreateProtector("Item");
            item.Name = dataProtector.Protect(item.Name);
            item.Description = dataProtector.Protect(item.Description);

            return item;
        }

        /// <summary>
        /// Unprotects the data in the Item object. This method uses IDataProtectionProvider to unprotect the data.
        /// Only the Name and Description properties are unprotected.
        /// </summary>
        /// <param name="item">this Item object</param>
        /// <returns>an EntryCategory object so that this method can be chained</returns>
        public static Item UnprotectData(this Item item)
        {
            var dataProtector = DataProtectionConfig.CreateProtector("Item");
            item.Name = dataProtector.Unprotect(item.Name);
            item.Description = dataProtector.Unprotect(item.Description);

            return item;
        }
    }
}