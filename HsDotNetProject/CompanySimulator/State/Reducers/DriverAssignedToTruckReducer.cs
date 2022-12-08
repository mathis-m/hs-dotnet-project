using CompanySimulator.State.Actions;
using TruckDriver.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.State.Reducers;

public class DriverAssignedToTruckReducer : IReducerT<AssignDriverToTruckPayload>
{
    public RootState Reduce(RootState currentState, AssignDriverToTruckPayload payload)
    {
        if (!currentState.CompanyState.Employees.Contains(payload.Driver)) throw new InvalidOperationException("Cannot assign a driver that is not employed");

        if (!currentState.CompanyState.OwnedTrucks.Contains(payload.Truck)) throw new InvalidOperationException("Cannot assign driver to a not owned truck");

        var currentAssignments = currentState.CompanyState.TruckAssignments;
        var derivedAssignments = new Dictionary<TruckOperator, Truck>(currentAssignments);

        if (derivedAssignments.ContainsValue(payload.Truck))
        {
            var (oldDriver, _) = derivedAssignments.First(x => x.Value == payload.Truck);

            derivedAssignments.Remove(oldDriver);
        }

        derivedAssignments[payload.Driver] = payload.Truck;

        var derivedCompanyState = currentState.CompanyState with
        {
            TruckAssignments = derivedAssignments,
        };

        return currentState with
        {
            CompanyState = derivedCompanyState,
        };
    }
}