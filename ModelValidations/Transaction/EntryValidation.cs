

using ErrorHandling;

using Microsoft.Extensions.Logging;

using Persistence.Models.Transaction;

namespace Persistence.ModelValidations.Transaction
{
    /// <summary>
    /// Validates the Entry model.
    /// </summary>
    public class EntryValidation : IModelValidation<Entry>
    {
        private readonly ILogger<EntryValidation> _logger;

        public EntryValidation(ILogger<EntryValidation> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Validates the Entry model.
        /// </summary>
        /// <param name="model">A Entry object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, ReturnValue.messages will contain a list of error messages.
        /// </returns>
        public ReturnValue ValidateModel(Entry model)
        {
            var returnValue = new ReturnValue();
            returnValue.AddMessageRangeToKey("entrydate", ValidateDate(model.EntryDate));
            returnValue.AddMessageRangeToKey("amount", ValidateAmount(model.Amount));
            returnValue.AddMessageRangeToKey("note", ValidateNote(model.Note));
            returnValue.AddMessageRangeToKey("itemid", ValidateItemId(model.ItemId));
            returnValue.AddMessageRangeToKey("categoryid", ValidateCategoryId(model.CategoryId));
                        
            returnValue.Success = returnValue.Messages.Count == 0;
            return returnValue;
        }

        /// <summary>
        /// Validates the Entry model asynchronously.
        /// </summary>
        /// <param name="model">An Entry object to be validated</param>
        /// <returns>
        /// A ReturnValue object with Success = true if the model is valid, false otherwise.
        /// If Success = false, ReturnValue.messages will contain a list of error messages.
        /// </returns>
        public async Task<ReturnValue> ValidateModelAsync(Entry model)
        {
            await Task.Yield();

            var returnValue = new ReturnValue();
            returnValue.AddMessageRangeToKey("entrydate", ValidateDate(model.EntryDate));
            returnValue.AddMessageRangeToKey("amount", ValidateAmount(model.Amount));
            returnValue.AddMessageRangeToKey("note", ValidateNote(model.Note));
            returnValue.AddMessageRangeToKey("itemid", ValidateItemId(model.ItemId));
            returnValue.AddMessageRangeToKey("categoryid", ValidateCategoryId(model.CategoryId));

            returnValue.Success = returnValue.Messages.Count == 0;
            return returnValue;
        }

        private static List<string> ValidateDate(DateTime date)
        {
            List<string> messages = new List<string>();
            if (date == DateTime.MinValue)
            {
                messages.Add("Date must be provided.");
            }
            return messages;
        }

        private static List<string> ValidateAmount(decimal amount)
        {
            List<string> messages = new List<string>();

            if (amount < 0.0m)
            {
                messages.Add("Amount must be greater than or equal to 0. If you want it to be an expense, then set this Entry as an expense.");
            }
            return messages;
        }
        private static List<string> ValidateNote(string note)
        {
            List<string> messages = new List<string>();
            var length = note.Length;
            if (length > 255)
            {
                messages.Add("Note must be less than 255 characters.");
            }
            if (string.IsNullOrWhiteSpace(note))
            {
                messages.Add("Note must be provided.");
            }
            return messages;
        }
        private static List<string> ValidateItemId(int itemId)
        {
            List<string> messages = new List<string>();
            if (itemId < 1)
            {
                messages.Add("ItemId must be greater than 0.");
            }
            return messages;
        }
        private static List<string> ValidateCategoryId(int categoryId)
        {
            List<string> messages = new List<string>();
            if (categoryId < 1)
            {
                messages.Add("CategoryId must be greater than 0.");
            }
            return messages;
        }
    }
}