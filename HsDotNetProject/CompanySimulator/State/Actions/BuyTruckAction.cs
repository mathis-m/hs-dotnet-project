using CompanySimulator.State.Reducers;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.State.Actions;

public record BuyTruckAction(Truck Payload) : ActionWithPayload<Truck>(Payload, typeof(TruckBoughtReducer));