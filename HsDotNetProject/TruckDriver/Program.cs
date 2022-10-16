using Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
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
    .AddSingleton<IGeneratorService<Name>, NameGeneratorService>()
    .AddSingleton<IGeneratorService<SalaryExpectation>, SalaryExpectationGeneratorService>()
    .AddSingleton<IGeneratorService<DriverCategory>, DriverCategoryGeneratorService>()
    .AddSingleton<IGeneratorService<TruckOperator>, TruckOperatorGeneratorService>();

var serviceProvider = services
    .BuildServiceProvider();

var truckOperatorGeneratorService = serviceProvider.GetRequiredService<IGeneratorService<TruckOperator>>();


var drivers = new List<TruckOperator>();

for (var i = 0; i < 5; i++) drivers.Add(await truckOperatorGeneratorService.GenerateAsync());

drivers.Print(driver =>
    $"{driver.Name.FullName}, {driver.SalaryExpectation.FormattedValueWithIsoCode}, {driver.DriverCategory.Type}"
);

Console.ReadLine();