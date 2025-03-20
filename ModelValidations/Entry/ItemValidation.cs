
using Persistence.Models.Entry;
using ErrorHandling;

namespace Persistence.ModelValidations.Entry
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
        /// If Success = false, the ReturnValue object will contain a list of error messages.
        /// </returns>
        public ReturnValue ValidateModel(Item model)
        {
            ReturnValue returnValue = new ReturnValue();
            List<string> messages = ValidateName(model.Name);
            if (messages.Count > 0)
            {
                returnValue.Messages.AddRange(messages);
            }
            messages = ValidateDescription(model.Description);
            if (messages.Count > 0)
            {
                returnValue.Messages.AddRange(messages);
            }
            messages = ValidateAmount(model.Amount);
            if (messages.Count > 0)
            {
                returnValue.Messages.AddRange(messages);
            }
            returnValue.Success = returnValue.Messages.Count == 0;
            return returnValue;
        }

        /// <summary>
        /// Validates the Item model asynchronously.
        /// </summary>
        /// <param name="model">the Item object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, the ReturnValue object will contain a list of error messages.
        /// </returns>
        public async Task<ReturnValue> ValidateModelAsync(Item model)
        {
            await Task.Yield();

            ReturnValue returnValue = new ReturnValue();
            List<string> messages = ValidateName(model.Name);
            if (messages.Count > 0)
            {
                returnValue.Messages.AddRange(messages);
            }
            messages = ValidateDescription(model.Description);
            if (messages.Count > 0)
            {
                returnValue.Messages.AddRange(messages);
            }
            messages = ValidateAmount(model.Amount);
            if (messages.Count > 0)
            {
                returnValue.Messages.AddRange(messages);
            }
            returnValue.Success = returnValue.Messages.Count == 0;
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
                messages.Add("Description is null or empty.");
            }
            if (description.Length > 500)
            {
                messages.Add("Description cannot be greater than 500 characters.");
            }

            return messages;
        }

        private static List<string> ValidateAmount(decimal amount)
        {
            List<string> messages = [];

            // TODO: add validation for amount
            return messages;
        }
    }
}