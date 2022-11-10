using Common.Services;
using FreightMarket.Factories;
using FreightMarket.Models;
using VehicleAcquisition.Models;

namespace FreightMarket.Services;

public class TransportationTenderRandomizerService : BaseRandomizerService<TransportationTender>
{
    private readonly IRandomizerService<TransportationGoods> _transportationGoodsRandomizerService;
    private readonly IRandomizerService<Location>            _locationRandomizerService;
    private readonly IDeliveryCalculator                     _deliveryCalculator;

    public TransportationTenderRandomizerService(IRandomizerService<TransportationGoods> transportationGoodsRandomizerService,
        IRandomizerService<Location> locationRandomizerService, IDeliveryCalculator deliveryCalculator)
    {
        _locationRandomizerService            = locationRandomizerService;
        _deliveryCalculator                   = deliveryCalculator;
        _transportationGoodsRandomizerService = transportationGoodsRandomizerService;
    }


    public override async Task<TransportationTender> NextAsync()
    {
        var transportationGoods = await _transportationGoodsRandomizerService.NextAsync();
        var startLocation       = await _locationRandomizerService.NextAsync();
        var targetLocation      = await _locationRandomizerService.NextAsync();

        var deliveryCharacteristics        = DeliveryCharacteristicsFactory.FromGoodType(transportationGoods.Type);
        var (deliveryDate, durationInDays) = _deliveryCalculator.CalculateDeliveryDateFromNow(deliveryCharacteristics);

        var tenderCompensation = TenderCompensationFactory.From(transportationGoods, durationInDays, deliveryCharacteristics);
        var tenderPenalty      = TenderPenaltyFactory.FromCompensation(tenderCompensation);

        return new TransportationTender(
            transportationGoods, 
            startLocation, 
            targetLocation, 
            deliveryDate,
            tenderCompensation,
            tenderPenalty
        );
    }
}