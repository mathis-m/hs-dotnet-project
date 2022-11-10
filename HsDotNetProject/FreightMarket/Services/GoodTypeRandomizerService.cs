using Common.Services;
using FreightMarket.Models;

namespace FreightMarket.Services;

public class GoodTypeRandomizerService : BaseRandomizerService<GoodTypes>
{
    private static readonly IEnumerable<GoodTypes> AvailableTypes = new List<GoodTypes>
    {
        GoodTypes.Cigarettes,
        GoodTypes.Textiles,
        GoodTypes.Chocolate,
        GoodTypes.Fruits,
        GoodTypes.IceCream,
        GoodTypes.Meat,
        GoodTypes.CrudeOil,
        GoodTypes.FuelOil,
        GoodTypes.Gasoline,
    };
    public override Task<GoodTypes> NextAsync()
    {
        return Task.FromResult(GetRandomItem(AvailableTypes));
    }
}
