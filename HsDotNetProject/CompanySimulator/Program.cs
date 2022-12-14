using Common.BngUtils;
using Common.Services;
using CompanySimulator.Factories;
using CompanySimulator.State;
using CompanySimulator.State.Reducers;
using CompanySimulator.UI;
using CompanySimulator.UI.Pages;
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
                    DriverCategory.OldButExperiencedType,
                    DriverCategory.RacerType,
                    DriverCategory.DreamyType,
                    DriverCategory.LovesHisJobType,
                    DriverCategory.Inconspicuous,
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
        services
            .AddSingleton<IDeliveryCalculator, DeliveryCalculator>()
            .AddSingleton<IRelocationStatsFactory, RelocationStatsFactory>()
            .AddSingleton<IBngDistanceCalculator, BngDistanceCalculator>();
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
            .AddSingleton<DriverHiredReducer>()
            .AddSingleton<TenderAcceptedReducer>()
            .AddSingleton<RoundEndedReducer>()
            .AddSingleton<DriverAssignedToTruckReducer>()
            .AddSingleton<TruckRelocationRequestedReducer>()
            .AddSingleton<TenderAssignedToTruckReducer>()
            .AddSingleton<TruckBoughtReducer>();
    }

    private static void RegisterPages(IServiceCollection services)
    {
        services
            .AddTransient<CompanyNamePromptPage>()
            .AddTransient<BuyTruckPage>()
            .AddTransient<HireDriverPage>()
            .AddTransient<AcceptTenderPage>()
            .AddTransient<AssignDriverToTruckPage>()
            .AddTransient<RelocateTruckPage>()
            .AddTransient<AssignTenderPage>()
            .AddTransient<MainMenuPage>();
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