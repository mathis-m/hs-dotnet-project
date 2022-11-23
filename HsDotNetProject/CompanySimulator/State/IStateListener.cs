namespace CompanySimulator.State;

public interface IStateListener
{
    void OnStateChanged(RootState oldState, RootState newState);
}