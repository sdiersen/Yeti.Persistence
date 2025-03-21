//TODO: add data protection to the Entry class for all data types

namespace Persistence.Models.Transaction
{
    /// <summary>
    /// This is a junction table between Category and Item.
    /// This table is used to associate categories with their items.
    /// This is a One-to-Many relationship between Category and Item.
    /// </summary>
    public class CategoryItem : AbstractBaseModel
    {
        /// <summary>
        /// The foreign key to the Item table.
        /// The default is -1.
        /// </summary>
        public int ItemId { get; set; } = -1;
        /// <summary>
        /// The foreign key to the Category table.
        /// The default is -1.
        /// </summary>
        public int CategoryId { get; set; } = -1;

        /// <summary>
        /// The default CategoryItem object
        /// </summary>
        /// <returns>A CategoryItem object with Id = -1</returns>
        public static CategoryItem DefaultCategoryItem()
        {
            return new CategoryItem { Id = -1 };
        }
    }
}