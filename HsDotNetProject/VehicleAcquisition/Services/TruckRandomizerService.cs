using Common.Services;
using VehicleAcquisition.Factories;
using VehicleAcquisition.Models;
using VehicleAcquisition.Models.Trucks;

namespace VehicleAcquisition.Services;

public class TruckRandomizerService : BaseRandomizerService<Truck>
{
    private static readonly IEnumerable<TruckTypes> AvailableTruckTypes = new List<TruckTypes>
        { TruckTypes.TankerTruck, TruckTypes.FlatbedTruck, TruckTypes.RefrigeratedTruck };

    private readonly IRandomizerService<Age>      _ageRandomizerService;
    private readonly IRandomizerService<Location> _locationRandomizerService;
    private readonly IRandomizerService<Size>     _sizeRandomizerService;

    public TruckRandomizerService(IRandomizerService<Age> ageRandomizerService, IRandomizerService<Location> locationRandomizerService,
        IRandomizerService<Size> sizeRandomizerService)
    {
        _ageRandomizerService      = ageRandomizerService;
        _locationRandomizerService = locationRandomizerService;
        _sizeRandomizerService     = sizeRandomizerService;
    }

    public override async Task<Truck> NextAsync()
    {
        var size     = await _sizeRandomizerService.NextAsync();
        var age      = await _ageRandomizerService.NextAsync();
        var location = await _locationRandomizerService.NextAsync();

        Truck truck = GetRandomItem(AvailableTruckTypes) switch
        {
            TruckTypes.RefrigeratedTruck => new RefrigeratedTruck(size, age, location),
            TruckTypes.FlatbedTruck => new FlatbedTruck(size, age, location),
            TruckTypes.TankerTruck => new TankerTruck(size, age, location),
            _ => new RefrigeratedTruck(size, age, location),
        };

        return truck;
    }
}