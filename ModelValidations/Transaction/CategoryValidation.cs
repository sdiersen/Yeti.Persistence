using Persistence.Models.Transaction;
using ErrorHandling;


namespace Persistence.ModelValidations.Transaction
{
    /// <summary>
    /// Validates the Category model.
    /// </summary>
    public class CategoryValidation : IModelValidation<Category>
    {
        /// <summary>
        /// Validates the Category model.
        /// </summary>
        /// <param name="model">the Category object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, ReturnValue.messages will contain a list of error messages.
        /// </returns>
        public ReturnValue ValidateModel(Category model)
        {

            var messages = new List<string>();
            messages.AddRange(ValidateName(model.Name));
            messages.AddRange(ValidateDescription(model.Description));
            messages = ValidateDescription(model.Description);

            var returnValue = new ReturnValue();
            returnValue.Success = messages.Count == 0;
            returnValue.Messages.AddRange(messages);
            return returnValue;
        }

        /// <summary>
        /// Validates the Category model asynchronously.
        /// </summary>
        /// <param name="model">the Category object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, ReturnValue.messages will contain a list of error messages.
        /// </returns>
        public async Task<ReturnValue> ValidateModelAsync(Category model)
        {
            await Task.Yield();

            var messages = new List<string>();
            messages.AddRange(ValidateName(model.Name));
            messages.AddRange(ValidateDescription(model.Description));
            messages = ValidateDescription(model.Description);

            var returnValue = new ReturnValue();
            returnValue.Success = messages.Count == 0;
            returnValue.Messages.AddRange(messages);
            return returnValue;
        }

        private static List<string> ValidateName(string name)
        {
            List<string> messages = [];
            if (string.IsNullOrWhiteSpace(name))
            {
                messages.Add("Name is null or empty.");
            }
            if (name.Length > 50)
            {
                messages.Add("Name cannot be greater than 50 characters.");
            }

            return messages;
        }

        private static List<string> ValidateDescription(string description)
        {
            List<string> messages = [];
            if (string.IsNullOrWhiteSpace(description))
            {
                messages.Add("Entry categories need a description.");
            }
            if (description.Length > 500)
            {
                messages.Add("Description cannot be greater than 500 characters.");
            }

            return messages;
        }
    }
}