using CompanySimulator.State;

namespace CompanySimulator.Factories;

public interface IInitialStateFactory
{
    Task<RootState> CreateInitialState();
}