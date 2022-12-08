using CompanySimulator.State.Reducers;

namespace CompanySimulator.State.Actions;

public record RequestTruckRelocationAction(RequestTruckRelocationPayload Payload) : ActionWithPayload<RequestTruckRelocationPayload>(Payload,
    typeof(TruckRelocationRequestedReducer));