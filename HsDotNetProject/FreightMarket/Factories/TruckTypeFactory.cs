using FreightMarket.Models;
using VehicleAcquisition.Factories;

namespace FreightMarket.Factories;

public static class TruckTypeFactory
{
    public static TruckTypes FromGoodType(GoodTypes goodType)
    {
        return goodType switch
        {
            GoodTypes.Cigarettes => TruckTypes.FlatbedTruck,
            GoodTypes.Textiles => TruckTypes.FlatbedTruck,
            GoodTypes.Chocolate => TruckTypes.FlatbedTruck,
            GoodTypes.Fruits => TruckTypes.RefrigeratedTruck,
            GoodTypes.IceCream => TruckTypes.RefrigeratedTruck,
            GoodTypes.Meat => TruckTypes.RefrigeratedTruck,
            GoodTypes.CrudeOil => TruckTypes.TankerTruck,
            GoodTypes.FuelOil => TruckTypes.TankerTruck,
            GoodTypes.Gasoline => TruckTypes.TankerTruck,
            _ => throw new ArgumentOutOfRangeException(nameof(goodType), goodType, null)
        };
    }
}