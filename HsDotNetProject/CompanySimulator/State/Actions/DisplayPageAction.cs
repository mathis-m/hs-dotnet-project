using CompanySimulator.State.Reducers;

namespace CompanySimulator.State.Actions;

public record DisplayPageAction(Pages Payload) : ActionWithPayload<Pages>(Payload, typeof(PageChangedReducer));