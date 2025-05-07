using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

using Persistence.Repositories.Identity;
using Persistence.Repositories.Transaction;

namespace Persistence.Repositories;

public class RepositoryFactory
{
    private readonly ILoggerFactory _loggerFactory;
    public RepositoryFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    //**********************************************************************************************
    // Account Repositores
    //**********************************************************************************************
    public AccountRepository CreateAccountRepository(SqlConnection connection, SqlTransaction? transaction)
    {
        var logger = _loggerFactory.CreateLogger<AccountRepository>();
        return new AccountRepository(logger, connection, transaction);
    }
    public AccountRoleRepository CreateAccountRoleRepository(SqlConnection connection, SqlTransaction? transaction)
    {
        var logger = _loggerFactory.CreateLogger<AccountRoleRepository>();
        return new AccountRoleRepository(logger, connection, transaction);
    }
    public RoleRepository CreateRoleRepository(SqlConnection connection, SqlTransaction? transaction)
    {
        var logger = _loggerFactory.CreateLogger<RoleRepository>();
        return new RoleRepository(logger, connection, transaction);
    }
    public UserDataRepository CreateUserDataRepository(SqlConnection connection, SqlTransaction? transaction)
    {
        var logger = _loggerFactory.CreateLogger<UserDataRepository>();
        return new UserDataRepository(logger, connection, transaction);
    }

    //**********************************************************************************************
    // Transaction Repositories
    //**********************************************************************************************
    public CategoryRepository CreateCategoryRepository(SqlConnection connection, SqlTransaction? transaction)
    {
        var logger = _loggerFactory.CreateLogger<CategoryRepository>();
        return new CategoryRepository(logger, connection, transaction);
    }
    public EntryRepository CreateEntryRepository(SqlConnection connection, SqlTransaction? transaction)
    {
        var logger = _loggerFactory.CreateLogger<EntryRepository>();
        return new EntryRepository(logger, connection, transaction);
    }
    public ItemRepository CreateItemRepository(SqlConnection connection, SqlTransaction? transaction)
    {
        var logger = _loggerFactory.CreateLogger<ItemRepository>();
        return new ItemRepository(logger, connection, transaction);
    }
    
    
}

