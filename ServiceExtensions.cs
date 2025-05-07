using Microsoft.Extensions.DependencyInjection;
using Persistence.Models.Transaction;
using Persistence.Models.Identity;
using Persistence.ModelValidations;
using Persistence.ModelValidations.Transaction;
using Persistence.ModelValidations.Identity;
using Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Persistence.Services.Identity;
using Persistence.Services.Transaction;

namespace Persistence;

/// <summary>
/// Extension methods for the IServiceCollection interface.
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Adds custom model validation services to the service collection.
    /// </summary>
    /// <param name="services">IServiceCollection object</param>
    public static void CustomModelValidationServices(this IServiceCollection services)
    {
        // Check if the services are already registered
        if (services.Any(sd => sd.ServiceType == typeof(IModelValidation<Category>)))
        {
            return;
        }

        //Entry Services
        services.AddScoped<IModelValidation<Category>, CategoryValidation>();
        services.AddScoped<IModelValidation<Item>, ItemValidation>();
        services.AddScoped<IModelValidation<Entry>, EntryValidation>();

        //Identity Services
        services.AddScoped<IModelValidation<UserData>, UserDataValidation>();
        services.AddScoped<IModelValidation<Account>, AccountValidation>();
        services.AddScoped<IModelValidation<Role>, RoleValidation>();
        services.AddScoped<IModelValidation<AccountRole>, AccountRoleValidation>();

        //Repository Factory
        services.AddSingleton<RepositoryFactory>();
    }
    public static void CustomPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Make sure ILogger is registered
        if (!services.Any(sd => sd.ServiceType == typeof(ILoggerFactory)))
        {
            throw new InvalidOperationException("ILogger must be registered before registering CustomPersistenceServices.");
        }

        //Add Model Validations
        services.CustomModelValidationServices();

        //Add Factories
        services.AddSingleton<RepositoryFactory>();
        services.AddSingleton<ModelValidationFactory>();

        // Set default connection string for UnitOfWork
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }
        UnitOfWork.SetDefaultConnectionString(connectionString);
        UnitOfWorkAsync.SetDefaultConnectionString(connectionString);

        // Add UnitOfWork
        services.AddTransient<UnitOfWork>(provider => UnitOfWork.Create());

        // Add UnitOfWorkAsync
        services.AddTransient(async provider => await UnitOfWorkAsync.CreateAsync());

        //Add Database Services
        //Identity Services
        services.AddTransient<IAccountServices>(provider => new AccountServices(
            provider.GetRequiredService<ILogger<AccountServices>>(),
            provider.GetRequiredService<RepositoryFactory>(),
            provider.GetRequiredService<ModelValidationFactory>()));

        services.AddTransient<IRoleServices>(provider => new RoleServices(
            provider.GetRequiredService<ILogger<RoleServices>>(),
            provider.GetRequiredService<RepositoryFactory>(),
            provider.GetRequiredService<ModelValidationFactory>()));

        //Transaction Services (probably need a better name)
        services.AddTransient<ICategoryServices>(provider => new CategoryServices(
            provider.GetRequiredService<ILogger<CategoryServices>>(),
            provider.GetRequiredService<RepositoryFactory>(),
            provider.GetRequiredService<ModelValidationFactory>()));

        services.AddTransient<IItemServices>(provider => new ItemServices(
            provider.GetRequiredService<ILogger<ItemServices>>(),
            provider.GetRequiredService<RepositoryFactory>(),
            provider.GetRequiredService<ModelValidationFactory>()));

        services.AddTransient<IEntryServices>(provider => new EntryServices(
            provider.GetRequiredService<ILogger<EntryServices>>(),
            provider.GetRequiredService<RepositoryFactory>(),
            provider.GetRequiredService<ModelValidationFactory>()));
    }
}



