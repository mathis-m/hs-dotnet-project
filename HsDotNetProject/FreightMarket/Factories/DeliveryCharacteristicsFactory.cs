using FreightMarket.Models;

namespace FreightMarket.Factories;

public static class DeliveryCharacteristicsFactory
{
    public static DeliveryCharacteristics FromGoodType(GoodTypes type)
    {
        return type switch
        {
            GoodTypes.Cigarettes => new DeliveryCharacteristics(3, 20),
            GoodTypes.Textiles => new DeliveryCharacteristics(3, 20),
            GoodTypes.Chocolate => new DeliveryCharacteristics(3, 10),
            GoodTypes.Fruits => new DeliveryCharacteristics(3, 14),
            GoodTypes.IceCream => new DeliveryCharacteristics(3, 10),
            GoodTypes.Meat => new DeliveryCharacteristics(3, 14),
            GoodTypes.CrudeOil => new DeliveryCharacteristics(3, 14),
            GoodTypes.FuelOil => new DeliveryCharacteristics(3, 25),
            GoodTypes.Gasoline => new DeliveryCharacteristics(3, 28),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };
    }
}