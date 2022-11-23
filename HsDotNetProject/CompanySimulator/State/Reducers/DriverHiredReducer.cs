using TruckDriver.Models;

namespace CompanySimulator.State.Reducers;

public class DriverHiredReducer : IReducerT<TruckOperator>
{
    public RootState Reduce(RootState currentState, TruckOperator driver)
    {
        if (driver is null) throw new ArgumentNullException(nameof(driver));

        var isOperatorJobSeeking = currentState.SimulationState.JobSeekingTruckDrivers.Contains(driver);
        if (!isOperatorJobSeeking) throw new ArgumentOutOfRangeException($"{driver} is not job seeking");

        var derivedJobSeekingDrivers = new List<TruckOperator>(currentState.SimulationState.JobSeekingTruckDrivers);
        derivedJobSeekingDrivers.Remove(driver);

        var derivedEmployees = new List<TruckOperator>(currentState.CompanyState.Employees) { driver };

        var derivedSimulationState = currentState.SimulationState with { JobSeekingTruckDrivers = derivedJobSeekingDrivers };
        var derivedCompanyState    = currentState.CompanyState with { Employees = derivedEmployees };

        return currentState with
        {
            SimulationState = derivedSimulationState,
            CompanyState = derivedCompanyState,
        };
    }
}