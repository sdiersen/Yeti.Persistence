
using Persistence.Models.Transaction;
using ErrorHandling;

namespace Persistence.ModelValidations.Transaction
{
    /// <summary>
    /// Validates the Item model.
    /// </summary>
    public class ItemValidation : IModelValidation<Item>
    {
        /// <summary>
        /// Validates the Item model.
        /// </summary>
        /// <param name="model">the Item object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, ReturnValue.messages will contain a list of error messages.
        /// </returns>
        public ReturnValue ValidateModel(Item model)
        {
            var messages = new List<string>();

            messages.AddRange(ValidateName(model.Name));
            messages.AddRange(ValidateNote(model.Note));
            messages.AddRange(ValidateBudgetAmount(model.BudgetAmount));
            messages.AddRange(ValidateCategoryId(model.CategoryId));

            var returnValue = new ReturnValue();
            returnValue.Success = messages.Count == 0;
            returnValue.Messages.AddRange(messages);

            return returnValue;
        }

        /// <summary>
        /// Validates the Item model asynchronously.
        /// </summary>
        /// <param name="model">the Item object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, ReturnValue.messages will contain a list of error messages.
        /// </returns>
        public async Task<ReturnValue> ValidateModelAsync(Item model)
        {
            await Task.Yield();

            var messages = new List<string>();

            messages.AddRange(ValidateName(model.Name));
            messages.AddRange(ValidateNote(model.Note));
            messages.AddRange(ValidateBudgetAmount(model.BudgetAmount));
            messages.AddRange(ValidateCategoryId(model.CategoryId));

            var returnValue = new ReturnValue();
            returnValue.Success = messages.Count == 0;
            returnValue.Messages.AddRange(messages);

            return returnValue;
        }

        private static List<string> ValidateName(string name)
        {
            var messages = new List<string>();
            if (string.IsNullOrWhiteSpace(name))
            {
                messages.Add("Name is null or empty.");
            }
            if (name.Length > 255)
            {
                messages.Add("Name cannot be greater than 255 characters.");
            }
            return messages;
        }
        private static List<string> ValidateNote(string note)
        {
            var messages = new List<string>();
            if (string.IsNullOrWhiteSpace(note))
            {
                messages.Add("Description is null or empty.");
            }
            if (note.Length > 255)
            {
                messages.Add("Description cannot be greater than 255 characters.");
            }


            return messages;
        }

        private static List<string> ValidateBudgetAmount(decimal amount)
        {
            var messages = new List<string>();
            if (amount < 0.0m)
            {
                messages.Add("Budget Amount must be greater than or equal to 0.");
            }
            return messages;
        }

        private static List<string> ValidateCategoryId(int categoryId)
        {
            var messages = new List<string>();
            if (categoryId < 1)
            {
                messages.Add("CategoryId must be greater than 0.");
            }
            return messages;
        }
    }
}