namespace CompanySimulator.State.Reducers;

public class RoundEndedReducer : IReducer
{
    public RootState Reduce(RootState currentState)
    {
        var currentDate = currentState.SimulationState.SimulationDate;

        var derivedSimulationState = currentState.SimulationState with
        {
            SimulationDate = currentDate.Add(TimeSpan.FromDays(1)),
        };

        return currentState with
        {
            SimulationState = derivedSimulationState,
        };
    }
}