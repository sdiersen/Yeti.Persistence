using Microsoft.Extensions.Logging;

using Persistence.ModelValidations.Identity;
using Persistence.ModelValidations.Transaction;

namespace Persistence.ModelValidations
{
    public class ModelValidationFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public ModelValidationFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        //**********************************************************************************************
        // Identity Validations
        //**********************************************************************************************
        public AccountValidation CreateAccountValidation()
        {
            var logger = _loggerFactory.CreateLogger<AccountValidation>();
            return new AccountValidation(logger);
        }

        public AccountRoleValidation CreateAccountRoleValidation()
        {
            var logger = _loggerFactory.CreateLogger<AccountRoleValidation>();
            return new AccountRoleValidation(logger);
        }

        public RoleValidation CreateRoleValidation()
        {
            var logger = _loggerFactory.CreateLogger<RoleValidation>();
            return new RoleValidation(logger);
        }

        public UserDataValidation CreateUserDataValidation()
        {
            var logger = _loggerFactory.CreateLogger<UserDataValidation>();
            return new UserDataValidation(logger);
        }

        //**********************************************************************************************
        // Transaction Validations
        //**********************************************************************************************
        public CategoryValidation CreateCategoryValidation()
        {
            var logger = _loggerFactory.CreateLogger<CategoryValidation>();
            return new CategoryValidation(logger);
        }

        public ItemValidation CreateItemValidation()
        {
            var logger = _loggerFactory.CreateLogger<ItemValidation>();
            return new ItemValidation(logger);
        }

        public EntryValidation CreateEntryValidation()
        {
            var logger = _loggerFactory.CreateLogger<EntryValidation>();
            return new EntryValidation(logger);
        }

        public CategoryItemValidation CreateCategoryItemValidation()
        {
            var logger = _loggerFactory.CreateLogger<CategoryItemValidation>();
            return new CategoryItemValidation(logger);
        }
    }
}
