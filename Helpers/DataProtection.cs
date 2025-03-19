using Microsoft.AspNetCore.DataProtection;

namespace Persistence.Helpers
{
    internal static class DataProtectionConfig
    {
        private static IDataProtectionProvider? _protectionProvider;

        public static void Initialize(IDataProtectionProvider protectionProvider)
        {
            _protectionProvider = protectionProvider ?? throw new ArgumentNullException(nameof(protectionProvider));
        }

        public static IDataProtector CreateProtector(string purpose)
        {
            if (_protectionProvider == null)
                throw new InvalidOperationException("Data Protection Provider is not initialized.");
            return _protectionProvider.CreateProtector(purpose);
        }
    }
}
