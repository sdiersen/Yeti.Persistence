using ErrorHandling;

using Persistence.Models.Identity;

namespace Persistence.ModelValidations.Identity
{
    /// <summary>
    /// Validates the UserData model.
    /// </summary>
    public class UserDataValidation : IModelValidation<UserData>
    {
        /// <summary>
        /// Validates the UserData model.
        /// </summary>
        /// <param name="model">the UserData object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, the ReturnValue object will contain a list of error messages.
        /// </returns>
        public ReturnValue ValidateModel(UserData model)
        {
            ReturnValue returnValue = new ReturnValue();

            returnValue.AddMessageRangeToKey("firstname", ValidateFirstName(model.FirstName));
            returnValue.AddMessageRangeToKey("lastname", ValidateLastName(model.LastName));
            returnValue.AddMessageRangeToKey("dob", ValidateDoB(model.DateOfBirth));

            returnValue.Success = returnValue.Messages.Count == 0;
            return returnValue;
        }

        /// <summary>
        /// Validates the UserData model asynchronously.
        /// </summary>
        /// <param name="model">the UserData object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, the ReturnValue object will contain a list of error messages.
        /// </returns>
        public async Task<ReturnValue> ValidateModelAsync(UserData model)
        {
            await Task.Yield();

            ReturnValue returnValue = new ReturnValue();

            returnValue.AddMessageRangeToKey("firstname", ValidateFirstName(model.FirstName));
            returnValue.AddMessageRangeToKey("lastname", ValidateLastName(model.LastName));
            returnValue.AddMessageRangeToKey("dob", ValidateDoB(model.DateOfBirth));

            returnValue.Success = returnValue.Messages.Count == 0;
            return returnValue;
        }

        private static List<string> ValidateFirstName(string name)
        {
            List<string> messages = [];
            if (string.IsNullOrWhiteSpace(name))
            {
                messages.Add("First name is null or empty.");
            }
            if (name.Length > 50)
            {
                messages.Add("First name is too long. Maximum length is 50 characters.");
            }
            return messages;
        }
        private static List<string> ValidateLastName(string name)
        {
            List<string> messages = [];
            if (string.IsNullOrWhiteSpace(name))
            {
                messages.Add("Last name is null or empty.");
            }
            if (name.Length > 50)
            {
                messages.Add("Last name is too long. Maximum length is 50 characters.");
            }
            return messages;
        }

        private static List<string> ValidateDoB(DateOnly? dateOfBirth)
        {
            List<string> messages = [];
            if (dateOfBirth == null)
            {
                messages.Add("Date of birth is null.");
            }
            else if (dateOfBirth > DateOnly.FromDateTime(DateTime.Now))
            {
                messages.Add("Date of birth cannot be in the future.");
            }
            return messages;
        }
    }
}