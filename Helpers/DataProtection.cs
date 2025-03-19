using Microsoft.AspNetCore.DataProtection;

namespace Persistence.Helpers
{
    internal static class DataProtectionConfig
    {
        private static IDataProtectionProvider? _protectionProvider;

        /// <summary>
        /// Initializes the Data Protection Provider.
        /// </summary>
        /// <param name="protectionProvider">IDataProtectionProvider object used to protect various database data</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Initialize(IDataProtectionProvider protectionProvider)
        {
            _protectionProvider = protectionProvider ?? throw new ArgumentNullException(nameof(protectionProvider));
        }

        /// <summary>
        /// Creates a Data Protector object for the specified purpose.
        /// </summary>
        /// <param name="purpose">The string used to help protect the data prior to insertion into the database.</param>
        /// <returns>An IDataProtector based on the purpose passed in.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IDataProtector CreateProtector(string purpose)
        {
            if (_protectionProvider == null)
                throw new InvalidOperationException("Data Protection Provider is not initialized.");
            return _protectionProvider.CreateProtector(purpose);
        }
    }
}
