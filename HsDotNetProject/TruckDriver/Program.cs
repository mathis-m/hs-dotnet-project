using Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using TruckDriver.Models;
using TruckDriver.Options;
using TruckDriver.Providers;
using TruckDriver.Services;

var sp = new ServiceCollection()
    .Configure<NameFromTextFileProviderConfig>(opt => { opt.FilePath = "names.txt"; })
    .Configure<DriverCategoryConfig>(opt =>
    {
        opt.AvailableTypes = new List<string>
        {
            "Old, but experienced",
            "Racer",
            "Dreamy",
            "Loves his job",
            "Inconspicuous"
        };
    })
    .Configure<SalaryExpectationLimitsConfig>(opt =>
    {
        opt.CurrencyIso = "EUR";
        opt.CurrencySymbol = "€";
        opt.LowerLimit = 2000;
        opt.UpperLimit = 5000;
    })
    .AddSingleton<INameProvider, NameFromTextFileProvider>()
    .AddSingleton<IGeneratorService<Name>, NameGeneratorService>()
    .AddSingleton<IGeneratorService<SalaryExpectation>, SalaryExpectationGeneratorService>()
    .AddSingleton<IGeneratorService<DriverCategory>, DriverCategoryGeneratorService>()
    .AddSingleton<IGeneratorService<TruckOperator>, TruckOperatorGeneratorService>()
    .BuildServiceProvider();

var truckOperatorGeneratorService = sp.GetRequiredService<IGeneratorService<TruckOperator>>();


var drivers = await truckOperatorGeneratorService.GenerateAsync(5);

drivers.Print(driver =>
    $"{driver.Name.FullName}, {driver.SalaryExpectation.FormattedValueWithIsoCode}, {driver.DriverCategory.Type}"
);

Console.ReadLine();