namespace CompanySimulator.State.Actions;

public abstract record ActionWithPayload<TPayload>(TPayload Payload, Type ReducerType) : ActionWithoutPayload(ReducerType);