using FreightMarket.Models;

namespace FreightMarket.Factories;

public static class TenderCompensationFactory
{
    private static readonly Random Random = new();

    public static TenderCompensation From(TransportationGoods goods, double deliveryDurationInDays, DeliveryCharacteristics deliveryCharacteristics)
    {
        var multiplier = Random.Next(2);
        var bonusFactor  = 1 + (0.2 + deliveryDurationInDays / deliveryCharacteristics.MaxDurationInDays) * multiplier;
        var compensation = goods.GoodPrice.MinPricePerTon * goods.WeightInTons * bonusFactor;
        return new TenderCompensation(compensation, goods.GoodPrice.Currency);
    }
}