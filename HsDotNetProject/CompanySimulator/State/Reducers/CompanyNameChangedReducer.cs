namespace CompanySimulator.State.Reducers;

public class CompanyNameChangedReducer : IReducerT<string>
{
    public RootState Reduce(RootState currentState, string name)
    {
        var derivedCompanyState = currentState.CompanyState with
        {
            Name = name,
        };

        return currentState with
        {
            CompanyState = derivedCompanyState,
        };
    }
}