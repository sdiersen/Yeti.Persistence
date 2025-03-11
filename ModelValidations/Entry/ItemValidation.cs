
using Persistence.Models.Entry;
using ErrorHandling;

namespace Persistence.ModelValidations.Entry
{
    public class ItemValidation : IModelValidation<Item>
    {
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

        public Task<ReturnValue> ValidateModelAsync(Item model)
        {
            return Task.Run(() => ValidateModel(model));
        }

        private List<string> ValidateName(string name)
        {
            List<string> messages = new List<string>();
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

        private List<string> ValidateDescription(string description)
        {
            List<string> messages = new List<string>();
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

        private List<string> ValidateAmount(decimal amount)
        {
            List<string> messages = new List<string>();

            // TODO: add validation for amount
            return messages;
        }
    }
}