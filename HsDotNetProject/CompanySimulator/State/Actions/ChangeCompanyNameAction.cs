using CompanySimulator.State.Reducers;

namespace CompanySimulator.State.Actions;

public record ChangeCompanyNameAction(string Payload) : ActionWithPayload<string>(Payload, typeof(CompanyNameChangedReducer));