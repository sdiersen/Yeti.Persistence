# Yeti.Persistence
Data Layer for the Yeti project

Example usage for DataHelpers:
```
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Helpers;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection()
    .AddSingleton<IConfiguration>(configuration)
    .AddSingleton<DatabaseHelpers>()
    .BuildServiceProvider();

var databaseHelpers = services.GetRequiredService<DatabaseHelpers>();

// Example usage
if (databaseHelpers.IsConnectionStringValid())
{
    Console.WriteLine("Connection string is valid.");
}

if (!databaseHelpers.IsDatabaseValid())
{
    databaseHelpers.CreateDatabase();
}

databaseHelpers.MigrateUp();
```

