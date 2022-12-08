using TruckDriver.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.State.Actions;

public record AssignDriverToTruckPayload(TruckOperator Driver, Truck Truck);