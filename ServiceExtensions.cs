using Microsoft.Extensions.DependencyInjection;
using Persistence.Models.Transaction;
using Persistence.Models.Identity;
using Persistence.ModelValidations;
using Persistence.ModelValidations.Transaction;
using Persistence.ModelValidations.Identity;
using Persistence.Repositories;

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
        //Entry Services
        services.AddScoped<IModelValidation<Category>, CategoryValidation>();
        services.AddScoped<IModelValidation<Item>, ItemValidation>();
        services.AddScoped<IModelValidation<Entry>, EntryValidation>();
        services.AddScoped<IModelValidation<CategoryItem>, CategoryItemValidation>();

        //Identity Services
        services.AddScoped<IModelValidation<UserData>, UserDataValidation>();
        services.AddScoped<IModelValidation<Account>, AccountValidation>();
        services.AddScoped<IModelValidation<Role>, RoleValidation>();
        services.AddScoped<IModelValidation<AccountRole>, AccountRoleValidation>();

        //Repository Factory
        services.AddSingleton<RepositoryFactory>();
    }
}



