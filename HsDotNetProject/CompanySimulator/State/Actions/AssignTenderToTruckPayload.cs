using FreightMarket.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.State.Actions;

public record AssignTenderToTruckPayload(TransportationTender Tender, Truck Truck);