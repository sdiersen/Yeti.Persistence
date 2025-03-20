using Persistence.Models.Entry;
using ErrorHandling;


namespace Persistence.ModelValidations.Entry
{
    /// <summary>
    /// Validates the EntryCategory model.
    /// </summary>
    public class EntryCategoryValidation : IModelValidation<EntryCategory>
    {
        /// <summary>
        /// Validates the EntryCategory model.
        /// </summary>
        /// <param name="model">the EntryCategory object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, the ReturnValue object will contain a list of error messages.
        /// </returns>
        public ReturnValue ValidateModel(EntryCategory model)
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
            returnValue.Success = returnValue.Messages.Count == 0;
            return returnValue;
        }

        /// <summary>
        /// Validates the EntryCategory model asynchronously.
        /// </summary>
        /// <param name="model">the EntryCategory object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, the ReturnValue object will contain a list of error messages.
        /// </returns>
        public async Task<ReturnValue> ValidateModelAsync(EntryCategory model)
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