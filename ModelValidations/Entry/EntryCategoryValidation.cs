using Persistence.Models.Entry;
using ErrorHandling;


namespace Persistence.ModelValidations.Entry
{
    public class EntryCategoryValidation : IModelValidation<EntryCategory>
    {

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

        public Task<ReturnValue> ValidateModelAsync(EntryCategory model)
        {
            throw new NotImplementedException();
        }

        //These private methods should probably return a list<string> or string[] of errors
        //since ReturnValue.Errors and ReturnValue.Messages are Lists of strings
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