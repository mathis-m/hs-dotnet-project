using Common.Services;
using VehicleAcquisition.Factories;

namespace VehicleAcquisition.Services;

public class TruckTypeRandomizerService : BaseRandomizerService<TruckTypes>
{
    private static readonly IEnumerable<TruckTypes> AvailableTypes = new List<TruckTypes> { TruckTypes.TankerTruck, TruckTypes.RefrigeratedTruck, TruckTypes.FlatbedTruck };

    public override Task<TruckTypes> NextAsync()
    {
        return Task.FromResult(GetRandomItem(AvailableTypes));
    }
}