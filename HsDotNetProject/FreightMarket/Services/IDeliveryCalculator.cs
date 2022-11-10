using FreightMarket.Models;

namespace FreightMarket.Services;

public interface IDeliveryCalculator
{
    (DateTime date, int dayCount) CalculateDeliveryDateFromNow(DeliveryCharacteristics deliveryCharacteristics);
}