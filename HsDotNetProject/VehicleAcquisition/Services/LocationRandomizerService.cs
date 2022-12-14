using Common.Services;
using VehicleAcquisition.Models;

namespace VehicleAcquisition.Services;

public class LocationRandomizerService : BaseRandomizerService<Location>
{
    public static readonly IReadOnlyList<Location> PossibleLocations = new List<Location>
    {
        new("Amsterdam"),
        new("Berlin"),
        new("Esslingen"),
        new("Rom"),
        new("Lissabon"),
        new("Istanbul"),
        new("Aarhus"),
        new("Tallinn"),
    };

    public override Task<Location> NextAsync()
    {
        return Task.FromResult(GetRandomItem(PossibleLocations));
    }
}