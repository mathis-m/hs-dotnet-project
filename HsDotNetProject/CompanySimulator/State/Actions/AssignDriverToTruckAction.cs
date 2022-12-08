using CompanySimulator.State.Reducers;

namespace CompanySimulator.State.Actions;

public record AssignDriverToTruckAction(AssignDriverToTruckPayload Payload) : ActionWithPayload<AssignDriverToTruckPayload>(Payload, typeof(DriverAssignedToTruckReducer));