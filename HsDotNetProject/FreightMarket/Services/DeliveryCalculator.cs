using FreightMarket.Models;

namespace FreightMarket.Services;

public class DeliveryCalculator : IDeliveryCalculator
{
    private static readonly Random DurationRandom = new();

    public (DateTime date, int dayCount) CalculateDeliveryDateFromNow(DeliveryCharacteristics deliveryCharacteristics)
    {
        var deliveryDurationInDays = DurationRandom.Next(deliveryCharacteristics.MinDurationInDays, deliveryCharacteristics.MaxDurationInDays + 1);
        return (date: DateTime.Now.AddDays(deliveryDurationInDays), dayCount: deliveryDurationInDays);
    }
}