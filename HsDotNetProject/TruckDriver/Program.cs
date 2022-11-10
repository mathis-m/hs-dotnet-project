using Common.Services;
using Microsoft.Extensions.DependencyInjection;
using TruckDriver;
using TruckDriver.Models;
using TruckDriver.Options;
using TruckDriver.Providers;
using TruckDriver.Repositories;
using TruckDriver.Services;

var services = new ServiceCollection();

// Setup Options
services
    .Configure<NameFromTextFileProviderConfig>(opt => { opt.FilePath = "names.txt"; })
    .Configure<DriverCategoryConfig>(opt =>
    {
        opt.AvailableTypes = new List<string>
        {
            "Old, but experienced",
            "Racer",
            "Dreamy",
            "Loves his job",
            "Inconspicuous",
        };
    })
    .Configure<SalaryExpectationLimitsConfig>(opt =>
    {
        opt.CurrencyIso    = "EUR";
        opt.CurrencySymbol = "€";
        opt.LowerLimit     = 2000;
        opt.UpperLimit     = 5000;
    });

// Setup Providers
services.AddSingleton<INameProvider, NameFromTextFileProvider>();

// Setup Repositories
services
    .AddSingleton<INameRepository, NameRepository>()
    .AddSingleton<ISalaryExpectationRepository, SalaryExpectationRepository>()
    .AddSingleton<IDriverCategoryRepository, DriverCategoryRepository>();

// Setup Generators
services
    .AddSingleton<IRandomizerService<Name>, NameRandomizerService>()
    .AddSingleton<IRandomizerService<SalaryExpectation>, SalaryExpectationRandomizerService>()
    .AddSingleton<IRandomizerService<DriverCategory>, DriverCategoryRandomizerService>()
    .AddSingleton<IRandomizerService<TruckOperator>, TruckOperatorRandomizerService>();

// Setup Console Application
services
    .AddSingleton<TruckOperatorListingApplication>();

var serviceProvider = services
    .BuildServiceProvider();

var app = serviceProvider.GetRequiredService<TruckOperatorListingApplication>();
await app.ExecuteAsync();

Console.ReadLine();