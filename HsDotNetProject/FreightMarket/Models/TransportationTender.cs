using VehicleAcquisition.Models;

namespace FreightMarket.Models;

public record TransportationTender(
    TransportationGoods TransportationGoods,
    Location StartLocation,
    Location TargetLocation,
    DateTime DeliveryDate,
    TenderCompensation Compensation,
    TenderPenalty Penalty
);