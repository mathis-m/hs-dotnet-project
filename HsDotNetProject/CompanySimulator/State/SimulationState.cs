using FreightMarket.Models;
using TruckDriver.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.State;

public record SimulationState(
    DateTime SimulationDate,
    IReadOnlyList<TransportationTender> OpenTenders,
    IReadOnlyList<TruckOperator> JobSeekingTruckDrivers,
    IReadOnlyList<Truck> AvailableTrucks
);