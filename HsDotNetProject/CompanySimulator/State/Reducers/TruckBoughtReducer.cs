using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.State.Reducers;

public class TruckBoughtReducer : IReducerT<Truck>
{
    public RootState Reduce(RootState currentState, Truck truck)
    {
        if (truck is null) throw new ArgumentNullException(nameof(truck));

        var isTruckAvailable = currentState.SimulationState.AvailableTrucks.Contains(truck);
        if (!isTruckAvailable) throw new ArgumentOutOfRangeException($"{truck} is not available");

        var derivedAvailableTrucks = new List<Truck>(currentState.SimulationState.AvailableTrucks);
        derivedAvailableTrucks.Remove(truck);

        var derivedOwnedTrucks = new List<Truck>(currentState.CompanyState.OwnedTrucks) { truck };

        var derivedSimulationState = currentState.SimulationState with { AvailableTrucks = derivedAvailableTrucks };
        var derivedCompanyState    = currentState.CompanyState with { OwnedTrucks = derivedOwnedTrucks };

        return currentState with
        {
            SimulationState = derivedSimulationState,
            CompanyState = derivedCompanyState,
        };
    }
}