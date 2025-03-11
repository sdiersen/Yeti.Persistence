using Microsoft.Extensions.DependencyInjection;
using Persistence.Models.Entry;
using Persistence.Models.Identity;
using Persistence.ModelValidations;
using Persistence.ModelValidations.Entry;
using Persistence.ModelValidations.Identity;

namespace Persistence
{
    public static class ServiceExtensions
    {
        public static void CustomModelValidationServices(this IServiceCollection services)
        {
            //Entry Services
            services.AddScoped<IModelValidation<EntryCategory>, EntryCategoryValidation>();
            services.AddScoped<IModelValidation<Item>, ItemValidation>();

            //Identity Services
            services.AddScoped<IModelValidation<UserData>, UserDataValidation>();
        }
    }
}