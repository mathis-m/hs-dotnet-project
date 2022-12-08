using CompanySimulator.Models;
using CompanySimulator.State.Actions;
using TruckDriver.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.State.Reducers;

public class TruckRelocationRequestedReducer : IReducerT<RequestTruckRelocationPayload>
{
    public RootState Reduce(RootState currentState, RequestTruckRelocationPayload payload)
    {
        if (!currentState.CompanyState.OwnedTrucks.Contains(payload.Truck)) throw new InvalidOperationException("Cannot place relocation request on a truck that is not owned");

        var currentRelocationRequests = currentState.CompanyState.TruckRelocationRequests;

        if (currentRelocationRequests.ContainsKey(payload.Truck))
        {
            var ongoingRelocation = currentRelocationRequests[payload.Truck];

            if (ongoingRelocation is { Status: not RelocationStatus.Arrived }) throw new InvalidOperationException("Cannot relocate truck with ongoing relocation");
        }


        var truckAssignments = new Dictionary<TruckOperator, Truck>(currentState.CompanyState.TruckAssignments);


        var relocationStatus = RelocationStatus.RelocationStarted;
        if (payload.Truck.Location == payload.Location)
            relocationStatus                                                      = RelocationStatus.Arrived;
        else if (!truckAssignments.ContainsValue(payload.Truck)) relocationStatus = RelocationStatus.WaitingForDriver;

        var derivedRelocationRequests = new Dictionary<Truck, RelocationRequest>(currentRelocationRequests)
        {
            [payload.Truck] = new(payload.Location, relocationStatus),
        };

        var derivedCompanyState = currentState.CompanyState with
        {
            TruckRelocationRequests = derivedRelocationRequests,
        };

        return currentState with
        {
            CompanyState = derivedCompanyState,
        };
    }
}