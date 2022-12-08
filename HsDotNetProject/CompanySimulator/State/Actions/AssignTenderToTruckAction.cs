using CompanySimulator.State.Reducers;

namespace CompanySimulator.State.Actions;

public record AssignTenderToTruckAction(AssignTenderToTruckPayload Payload) : ActionWithPayload<AssignTenderToTruckPayload>(Payload, typeof(TenderAssignedToTruckReducer));