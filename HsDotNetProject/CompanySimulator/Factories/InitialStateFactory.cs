using Common.Models;
using Common.Services;
using CompanySimulator.Models;
using CompanySimulator.State;
using FreightMarket.Models;
using TruckDriver.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.Factories;

public class InitialStateFactory : IInitialStateFactory
{
    private const    int                                      InitialTruckCount  = 8;
    private const    int                                      InitialDriverCount = 5;
    private const    int                                      InitialTenderCount = 8;
    private readonly IRandomizerService<TruckOperator>        _driverRandomizerService;
    private readonly IRandomizerService<TransportationTender> _tenderRandomizerService;

    private readonly IRandomizerService<Truck> _truckRandomizerService;

    public InitialStateFactory(
        IRandomizerService<Truck> truckRandomizerService,
        IRandomizerService<TruckOperator> driverRandomizerService,
        IRandomizerService<TransportationTender> tenderRandomizerService
    )
    {
        _truckRandomizerService  = truckRandomizerService;
        _driverRandomizerService = driverRandomizerService;
        _tenderRandomizerService = tenderRandomizerService;
    }

    public async Task<RootState> CreateInitialState()
    {
        var trucks       = await GenerateInitialTrucks();
        var truckDrivers = await GenerateInitialDrivers();
        var tenders      = await GenerateInitialTenders();

        return new RootState(
            new SimulationState(
                DateTime.Now,
                tenders,
                truckDrivers,
                trucks
            ),
            new CompanyState(
                "",
                new AccountBalance(50000, new Currency("EUR", "€")),
                new List<Truck>(),
                new List<TruckOperator>(),
                new List<TransportationTender>()
            ),
            new ApplicationState(
                Pages.CompanyNamePrompter
            )
        );
    }

    private async Task<List<Truck>> GenerateInitialTrucks()
    {
        var trucks = new List<Truck>();
        for (var i = 0; i < InitialTruckCount; i++)
        {
            var randomTruck = await _truckRandomizerService.NextAsync();
            trucks.Add(randomTruck);
        }

        return trucks;
    }

    private async Task<List<TruckOperator>> GenerateInitialDrivers()
    {
        var drivers = new List<TruckOperator>();
        for (var i = 0; i < InitialDriverCount; i++)
        {
            var randomDriver = await _driverRandomizerService.NextAsync();
            drivers.Add(randomDriver);
        }

        return drivers;
    }

    private async Task<List<TransportationTender>> GenerateInitialTenders()
    {
        var tenders = new List<TransportationTender>();
        for (var i = 0; i < InitialTenderCount; i++)
        {
            var randomTender = await _tenderRandomizerService.NextAsync();
            tenders.Add(randomTender);
        }

        return tenders;
    }
}