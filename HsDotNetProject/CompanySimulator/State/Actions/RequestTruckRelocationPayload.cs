using VehicleAcquisition.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.State.Actions;

public record RequestTruckRelocationPayload(Truck Truck, Location Location, double PayloadInTons = 0);