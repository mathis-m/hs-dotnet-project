using Common.Models;
using FreightMarket.Models;

namespace FreightMarket.Factories;

public static class GoodPriceFactory
{
    private static readonly Currency Eur = new("EUR", "€");

    public static GoodPrice FromGoodType(GoodTypes type)
    {
        return type switch
        {
            GoodTypes.Cigarettes => new GoodPrice(100, Eur),
            GoodTypes.Textiles => new GoodPrice(50, Eur),
            GoodTypes.Chocolate => new GoodPrice(120, Eur),
            GoodTypes.Fruits => new GoodPrice(150, Eur),
            GoodTypes.IceCream => new GoodPrice(180, Eur),
            GoodTypes.Meat => new GoodPrice(130, Eur),
            GoodTypes.CrudeOil => new GoodPrice(120, Eur),
            GoodTypes.FuelOil => new GoodPrice(90, Eur),
            GoodTypes.Gasoline => new GoodPrice(80, Eur),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };
    }
}