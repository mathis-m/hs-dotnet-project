using Common.Services;
using FreightMarket.Factories;
using FreightMarket.Models;
using VehicleAcquisition.Factories;
using VehicleAcquisition.Models;

namespace FreightMarket.Services;

public class TransportationGoodsRandomizerService : BaseRandomizerService<TransportationGoods>
{
    private static readonly Random                        WeightRandom = new();
    private readonly        IRandomizerService<GoodTypes> _goodTypeRandomizerService;

    public TransportationGoodsRandomizerService(IRandomizerService<GoodTypes> goodTypeRandomizerService)
    {
        _goodTypeRandomizerService = goodTypeRandomizerService;
    }

    public override async Task<TransportationGoods> NextAsync()
    {
        var type         = await _goodTypeRandomizerService.NextAsync();
        var truckType    = TruckTypeFactory.FromGoodType(type);
        var weightLimit  = MaxPayloadFactory.GetMaxPayloadFor(truckType, Size.ExtraLarge);
        var weightInTons = WeightRandom.Next(1, weightLimit + 1);
        var goodPrice    = GoodPriceFactory.FromGoodType(type);

        return new TransportationGoods(type, truckType, weightInTons, goodPrice);
    }
}