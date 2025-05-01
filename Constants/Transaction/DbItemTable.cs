namespace Persistence.Migrations.Constants
{
    /// <summary>
    /// Constant column names for the Item table.
    /// The item table represents items within a Category.
    /// For example, Household might be the Category and Electrical might be the Item.
    /// </summary>
    public class DbItemTable : DbCommonColumns
    {
        /// <summary>
        /// The name of a the item in a Category.
        /// </summary>
        public const string NAME = "Name";
        /// <summary>
        /// This is an optional entry that will be a descrption or note about the item.
        /// </summary>
        public const string NOTE = "Note";
        /// <summary>
        /// This is a boolean that indicates if the item is an expense or income.
        /// True = Expense, False = Income
        /// This is for the overall Item, individual entries will have their own IsExpense.
        /// </summary>
        public const string IS_EXPENSE = "IsExpense";
        /// <summary>
        /// This is the amount that is budgeted to this item.
        /// </summary>
        public const string BUDGET_AMOUNT = "BudgetAmount";
        /// <summary>
        /// This is the sum of the entries for this item.
        /// </summary>
        public const string CURRENT_AMOUNT = "CurrentAmount";
        /// <summary>
        /// This is a foreign key to the Category table.
        /// </summary>
        public const string CATEGORY_ID = "CategoryId";
    }
}