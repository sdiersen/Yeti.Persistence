namespace Persistence.Migrations.Constants
{
    /// <summary>
    /// Constant table names for the database.
    /// </summary>
    public static class DbTableNames
    {
        //****************************************************************************************************
        // Identity Tables
        //****************************************************************************************************
        /// <summary>
        /// The table that holds the user data.
        /// </summary>
        public const string USER_DATA_TABLE = "dbo.User_Data";

        //****************************************************************************************************
        // Entry Tables
        //****************************************************************************************************
        /// <summary>
        /// This table holds Items. These are specific buckets in a Category.
        /// i.e. Household might be the Category and Electrical might be the Item.
        /// </summary>
        public const string ITEM_TABLE = "dbo.Item";
        /// <summary>
        /// This is the Category table. It holds all of the categories. 
        /// i.e. Household, Entertainment, etc.
        /// </summary>
        public const string CATEGORY_TABLE = "dbo.Category";
        /// <summary>
        /// This table holds the relationship between Category and Item.
        /// </summary>
        public const string CATEGORY_ITEM_TABLE = "dbo.Category_Item";
        /// <summary>
        /// This table holds the entries. These are the transactions that happen.
        /// i.e. $20.00 for a movie ticket.
        /// </summary>
        public const string ENTRY_TABLE = "dbo.Entry";
    }
}