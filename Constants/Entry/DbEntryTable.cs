namespace Persistence.Migrations.Constants
{
    /// <summary>
    /// Constant column names for the Entry table.
    /// The Entry table represents a single entry of money into or out of the budget.
    /// </summary>
    public class DbEntryTable : DbCommonColumns
    {
        /// <summary>
        /// The date the transaction happened.
        /// </summary>
        public const string ENTRY_DATE = "EntryDate";
        /// <summary>
        /// The amount of the transaction.
        /// This can be positive or negative.
        /// </summary>
        public const string AMOUNT = "Amount";
        /// <summary>
        /// This is an optional entry that will be a descrption or note about the transaction.
        /// </summary>
        public const string NOTE = "Note";
        /// <summary>
        /// This is a foreign key to the Item table.
        /// </summary>
        public const string ITEM_ID = "ItemId";
        /// <summary>
        /// This is a foreign key to the Category table.
        /// </summary>
        public const string CATEGORY_ID = "CategoryId";
    }
}