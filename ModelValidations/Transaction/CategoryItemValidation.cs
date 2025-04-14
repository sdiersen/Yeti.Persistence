using ErrorHandling;

using Persistence.Models.Transaction;

namespace Persistence.ModelValidations.Transaction
{
    /// <summary>
    /// Validates the CategoryItem model.
    /// </summary>
    public class CategoryItemValidation : IModelValidation<CategoryItem>
    {
        /// <summary>
        /// Validates the CategoryItem model. Only tests that the Id's provided are in a valid format.
        /// Does not test the Id for the CategoryItem object itself.
        /// </summary>
        /// <param name="model">CategoryItem object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, ReturnValue.messages will contain a list of error messages.
        /// </returns>
        public ReturnValue ValidateModel(CategoryItem model)
        {
            ReturnValue returnValue = new ReturnValue();
            if (model.ItemId < 1)
            {
                returnValue.AddMessage("itemid", "ItemId must be greater than 0.");
            }
            if (model.CategoryId < 1)
            {
                returnValue.AddMessage("categoryid", "CategoryId must be greater than 0.");
            }
            returnValue.Success = returnValue.Messages.Count == 0;
            return returnValue;

        }

        /// <summary>
        /// Validates the CategoryItem model asynchronously. Only tests that the Id's provided are in a valid format.
        /// Does not test the Id for the CategoryItem object itself.
        /// </summary>
        /// <param name="model">CategoryItem object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, ReturnValue.messages will contain a list of error messages.
        /// </returns>

        public async Task<ReturnValue> ValidateModelAsync(CategoryItem model)
        {
            await Task.Yield();

            ReturnValue returnValue = new ReturnValue();
            if (model.ItemId < 1)
            {
                returnValue.AddMessage("itemid", "ItemId must be greater than 0.");
            }
            if (model.CategoryId < 1)
            {
                returnValue.AddMessage("categoryid", "CategoryId must be greater than 0.");
            }
            returnValue.Success = returnValue.Messages.Count == 0;
            return returnValue;
        }
    }
}