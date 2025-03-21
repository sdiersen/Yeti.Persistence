using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

using Persistence.Helpers;
using Persistence.Models.Transaction;
using Persistence.Models.Identity;
using Persistence.ModelValidations;
using Persistence.ModelValidations.Transaction;
using Persistence.ModelValidations.Identity;

namespace Persistence
{
    /// <summary>
    /// Extension methods for the IServiceCollection interface.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds custom model validation services to the service collection.
        /// </summary>
        /// <param name="services">IServiceCollection object</param>
        /// <exception cref="InvalidOperationException">Thrown when Data Protection Provider is not available.</exception>
        public static void CustomModelValidationServices(this IServiceCollection services)
        {
            //Entry Services
            services.AddScoped<IModelValidation<Category>, CategoryValidation>();
            services.AddScoped<IModelValidation<Item>, ItemValidation>();

            //Identity Services
            services.AddScoped<IModelValidation<UserData>, UserDataValidation>();

            // Add Security/Data Protection Services
            services.AddDataProtection();

            // Build the service provider and initialize any necessary services
            var serviceProvider = services.BuildServiceProvider();
            var protectionProvider = serviceProvider.GetService<IDataProtectionProvider>();

            if (protectionProvider == null)
            {
                throw new InvalidOperationException("Data Protection Provider is not available.");
            }
            MyLibraryInitializer.InitializeDataProtection(protectionProvider);

        }
    }

    //TODO : consider moving initializer logic to its own file
    internal static class MyLibraryInitializer
    {
        public static void InitializeDataProtection(IDataProtectionProvider protectionProvider)
        {
            DataProtectionConfig.Initialize(protectionProvider);
        }
    }
}



