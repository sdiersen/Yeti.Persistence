using Microsoft.AspNetCore.DataProtection;

using Persistence.Helpers;

namespace Persistence.Models.Transaction
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
        /// A description or note for the transaction.
        /// </summary>
        public string Note { get; set; } = string.Empty;
        /// <summary>
        /// A boolean that indicates if the transaction is an expense or income.
        /// True = Expense, False = Income
        /// Default is true (Expense).
        /// </summary>
        public Boolean IsExpense { get; set; } = true;
        /// <summary>
        /// The amount of money budgeted to this item.
        /// </summary>
        public decimal BudgetAmount { get; set; } = 0.0m;
        /// <summary>
        /// The sum of the entries for this item.
        /// </summary>
        public decimal CurrentAmount { get; set; } = 0.0m;
        /// <summary>
        /// The foreign key to the Category table.
        /// Default is -1.
        /// </summary>
        public int CategoryId { get; set; } = -1;

        /// <summary>
        /// The default item for an entry.
        /// </summary>
        /// <returns>An Item object with defaults set and Id = -1</returns>
        public static Item DefaultItem()
        {
            return new Item { Id = -1 };
        }
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
        /// Only the Name and Note properties are protected.
        /// amount protection is a problem for later.
        /// </summary>
        /// <param name="item">this Item object</param>
        /// <returns>an Item object so that this method can be chained</returns>
        public static Item ProtectData(this Item item)
        {
            var dataProtector = DataProtectionConfig.CreateProtector("Item");
            item.Name = dataProtector.Protect(item.Name);
            item.Note = dataProtector.Protect(item.Note);

            return item;
        }

        /// <summary>
        /// Unprotects the data in the Item object. This method uses IDataProtectionProvider to unprotect the data.
        /// Only the Name and Note properties are unprotected.
        /// </summary>
        /// <param name="item">this Item object</param>
        /// <returns>an EntryCategory object so that this method can be chained</returns>
        public static Item UnprotectData(this Item item)
        {
            var dataProtector = DataProtectionConfig.CreateProtector("Item");
            item.Name = dataProtector.Unprotect(item.Name);
            item.Note = dataProtector.Unprotect(item.Note);

            return item;
        }
    }
}