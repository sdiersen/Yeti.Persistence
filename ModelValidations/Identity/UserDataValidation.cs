
using System.Text.RegularExpressions;

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

            List<string> messages = ValidateUserName(model.Username);
            if (messages.Count > 0)
            {
                returnValue.Messages.AddRange(messages);
            }

            messages = ValidateEmail(model.Email);
            if (messages.Count > 0)
            {
                returnValue.Messages.AddRange(messages);
            }

            messages = ValidatePassword(model.Password);
            if (messages.Count > 0)
            {
                returnValue.Messages.AddRange(messages);
            }

            // if there are no errors
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
            List<string> messages = ValidateUserName(model.Username);
            if (messages.Count > 0)
            {
                returnValue.Messages.AddRange(messages);
            }
            messages = ValidateEmail(model.Email);
            if (messages.Count > 0)
            {
                returnValue.Messages.AddRange(messages);
            }
            messages = ValidatePassword(model.Password);
            if (messages.Count > 0)
            {
                returnValue.Messages.AddRange(messages);
            }
            returnValue.Success = returnValue.Messages.Count == 0;
            return returnValue;
        }

        private static List<string> ValidateUserName(string username)
        {
            List<string> messages = [];
            if (string.IsNullOrWhiteSpace(username))
            {
                messages.Add("Username is null or empty.");
            }
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9]{3,20}$"))
            {
                messages.Add("Username must be between 3 and 20 characters long and contain only letters and numbers.");
            }

            return messages;
        }

        private static List<string> ValidateEmail(string email)
        {
            List<string> messages = [];
            if (string.IsNullOrWhiteSpace(email))
            {
                messages.Add("Email is null or empty.");
            }
            if (!Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                messages.Add("Email is not in the correct format.");
            }

            return messages;
        }

        private static List<string> ValidatePassword(string password)
        {
            List<string> messages = [];
            if (string.IsNullOrWhiteSpace(password))
            {
                messages.Add("Password is null or empty.");
            }
            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$"))
            {
                messages.Add("Password must be between 8 and 15 characters long and contain at least one lowercase letter, one uppercase letter, one number, and one special character.");
            }

            return messages;
        }
    }
}