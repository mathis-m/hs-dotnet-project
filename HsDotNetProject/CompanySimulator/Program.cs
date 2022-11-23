using Common.Services;
using CompanySimulator.Factories;
using CompanySimulator.State;
using CompanySimulator.State.Reducers;
using CompanySimulator.UI;
using CompanySimulator.UI.MainMenu;
using FreightMarket.Models;
using FreightMarket.Services;
using Microsoft.Extensions.DependencyInjection;
using TruckDriver.Models;
using TruckDriver.Options;
using TruckDriver.Providers;
using TruckDriver.Repositories;
using TruckDriver.Services;
using VehicleAcquisition.Models;
using VehicleAcquisition.Models.Trucks;
using VehicleAcquisition.Services;

namespace CompanySimulator;

internal class Program
{
    private static void Main(string[] args)
    {
        var serviceProvider = ConfigureServices();

        var app = serviceProvider.GetRequiredService<App>();

        app.Execute();

        Console.ReadKey();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<App>();

        SetupOptions(services);
        RegisterProviders(services);
        RegisterRepositories(services);
        RegisterPages(services);
        RegisterStateRelated(services);
        RegisterRandomizer(services);
        RegisterHelpers(services);

        return services.BuildServiceProvider();
    }

    private static void SetupOptions(IServiceCollection services)
    {
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
    }

    private static void RegisterProviders(IServiceCollection services)
    {
        services.AddSingleton<INameProvider, NameFromTextFileProvider>();
    }

    private static void RegisterHelpers(IServiceCollection services)
    {
        services.AddSingleton<IDeliveryCalculator, DeliveryCalculator>();
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services
            .AddSingleton<INameRepository, NameRepository>()
            .AddSingleton<ISalaryExpectationRepository, SalaryExpectationRepository>()
            .AddSingleton<IDriverCategoryRepository, DriverCategoryRepository>();
    }

    private static void RegisterStateRelated(IServiceCollection services)
    {
        services
            .AddSingleton<StateManager>()
            .AddSingleton<IInitialStateFactory, InitialStateFactory>();

        RegisterStateReducers(services);
    }

    private static void RegisterStateReducers(IServiceCollection services)
    {
        services
            .AddSingleton<PageChangedReducer>()
            .AddSingleton<CompanyNameChangedReducer>()
            .AddSingleton<TruckBoughtReducer>();
    }

    private static void RegisterPages(IServiceCollection services)
    {
        services
            .AddSingleton<CompanyNamePromptPage>()
            .AddSingleton<MainMenuPage>();
    }

    private static void RegisterRandomizer(IServiceCollection services)
    {
        services
            .AddSingleton<IRandomizerService<Name>, NameRandomizerService>()
            .AddSingleton<IRandomizerService<SalaryExpectation>, SalaryExpectationRandomizerService>()
            .AddSingleton<IRandomizerService<DriverCategory>, DriverCategoryRandomizerService>()
            .AddSingleton<IRandomizerService<TruckOperator>, TruckOperatorRandomizerService>()
            .AddSingleton<IRandomizerService<Size>, TruckSizeRandomizerService>()
            .AddSingleton<IRandomizerService<Age>, TruckAgeRandomizerService>()
            .AddSingleton<IRandomizerService<Truck>, TruckRandomizerService>()
            .AddSingleton<IRandomizerService<Location>, LocationRandomizerService>()
            .AddSingleton<IRandomizerService<GoodTypes>, GoodTypeRandomizerService>()
            .AddSingleton<IRandomizerService<TransportationGoods>, TransportationGoodsRandomizerService>()
            .AddSingleton<IRandomizerService<TransportationTender>, TransportationTenderRandomizerService>();
    }
}