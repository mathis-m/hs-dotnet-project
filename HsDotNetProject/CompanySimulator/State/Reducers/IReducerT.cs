namespace CompanySimulator.State.Reducers;

public interface IReducerT<in TPayload>
{
    RootState Reduce(RootState currentState, TPayload payload);
}