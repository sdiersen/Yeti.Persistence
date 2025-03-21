//TODO: add data protection to the Entry class for all data types

namespace Persistence.Models.Transaction
{
    /// <summary>
    /// Represents a transaction that can be either an expense or an income.
    /// </summary>
    public class Entry : AbstractBaseModel
    {
        /// <summary>
        /// The date the transaction happened. This will default to when the object was created.
        /// This value should be the actual date of the transaction.
        /// </summary>
        public DateTime EntryDate { get; set; } = DateTime.Now;
        /// <summary>
        /// The amount of the transaction.
        /// This can be positive or negative.
        /// </summary>
        public decimal Amount { get; set; } = 0.0m;
        /// <summary>
        /// This is a boolean that indicates if the transaction is an expense or income.
        /// True = Expense, False = Income
        /// This is independnt of the Item's IsExpense.
        /// Default is true (Expense).
        /// </summary>
        public bool IsExpense { get; set; } = true;
        /// <summary>
        /// This is an optional entry that will be a descrption or note about the transaction.
        /// </summary>
        public string Note { get; set; } = string.Empty;
        /// <summary>
        /// The foreign key to the Item table.
        /// Default is -1.
        /// </summary>
        public int ItemId { get; set; } = -1;
        /// <summary>
        /// The foreign key to the Category table.
        /// Default is -1.
        /// </summary>
        public int CategoryId { get; set; } = -1;

        /// <summary>
        /// The default entry for an entry.
        /// </summary>
        /// <returns>An Entry object with defaults. Id defaults to -1.</returns>
        public static Entry DefaultEntry()
        {
            return new Entry { Id = -1 };
        }
    }
}