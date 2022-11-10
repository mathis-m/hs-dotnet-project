using VehicleAcquisition.Factories;

namespace FreightMarket.Models;

public record TransportationGoods(GoodTypes Type, TruckTypes TruckType, int WeightInTons, GoodPrice GoodPrice);