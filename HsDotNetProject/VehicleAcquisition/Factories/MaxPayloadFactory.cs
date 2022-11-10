using VehicleAcquisition.Models;

namespace VehicleAcquisition.Factories;

public class MaxPayloadFactory
{
    private static readonly Dictionary<TruckTypes, Dictionary<Size, int>> _maxPayoadSizes = new()
    {
        {
            TruckTypes.FlatbedTruck,
            new Dictionary<Size, int>
            {
                { Size.Small, 4 },
                { Size.Medium, 6 },
                { Size.Large, 7 },
                { Size.ExtraLarge, 10 },
            }
        },
        {
            TruckTypes.RefrigeratedTruck,
            new Dictionary<Size, int>
            {
                { Size.Small, 3 },
                { Size.Medium, 4 },
                { Size.Large, 5 },
                { Size.ExtraLarge, 6 },
            }
        },
        {
            TruckTypes.TankerTruck,
            new Dictionary<Size, int>
            {
                { Size.Small, 2 },
                { Size.Medium, 4 },
                { Size.Large, 8 },
                { Size.ExtraLarge, 10 },
            }
        },
    };

    public static int GetMaxPayloadFor(TruckTypes truckType, Size size)
    {
        var sizeMapping = _maxPayoadSizes.GetValueOrDefault(truckType);
        if (sizeMapping == null)
            throw new NotImplementedException($"Truck type {truckType} is currently not supported");

        var maxPayload = sizeMapping.GetValueOrDefault(size);
        if (maxPayload == default)
            throw new NotImplementedException($"Size {size} of truck type {truckType} is currently not supported");

        return maxPayload;
    }
}