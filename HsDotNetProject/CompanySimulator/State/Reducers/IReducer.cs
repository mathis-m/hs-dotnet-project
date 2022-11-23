namespace CompanySimulator.State.Reducers;

public interface IReducer
{
    RootState Reduce(RootState currentState);
}