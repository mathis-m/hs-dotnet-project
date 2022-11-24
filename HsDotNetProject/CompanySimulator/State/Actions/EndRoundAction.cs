using CompanySimulator.State.Reducers;

namespace CompanySimulator.State.Actions;

public record EndRoundAction() : ActionWithoutPayload(typeof(RoundEndedReducer));