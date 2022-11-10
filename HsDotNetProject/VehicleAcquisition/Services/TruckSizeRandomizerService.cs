using Common.Services;
using VehicleAcquisition.Models;

namespace VehicleAcquisition.Services;

public class TruckSizeRandomizerService : BaseRandomizerService<Size>
{
    private static readonly IEnumerable<Size> AvailableSizes = new List<Size> { Size.Small, Size.Medium, Size.Large, Size.ExtraLarge };

    public override Task<Size> NextAsync()
    {
        return Task.FromResult(GetRandomItem(AvailableSizes));
    }
}