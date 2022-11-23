namespace CompanySimulator.State.Reducers;

public class PageChangedReducer : IReducerT<Pages>
{
    public RootState Reduce(RootState currentState, Pages page)
    {
        var derivedApplicationState = currentState.ApplicationState with
        {
            CurrentPage = page,
        };

        return currentState with
        {
            ApplicationState = derivedApplicationState,
        };
    }
}