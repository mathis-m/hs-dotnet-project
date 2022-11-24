using FreightMarket.Models;

namespace CompanySimulator.State.Reducers;

public class TenderAcceptedReducer : IReducerT<TransportationTender>
{
    public RootState Reduce(RootState currentState, TransportationTender tender)
    {
        if (tender is null) throw new ArgumentNullException(nameof(tender));

        var isTenderAvailable = currentState.SimulationState.OpenTenders.Contains(tender);
        if (!isTenderAvailable) throw new ArgumentOutOfRangeException($"{tender} is not available");

        var derivedOpenTenders = new List<TransportationTender>(currentState.SimulationState.OpenTenders);
        derivedOpenTenders.Remove(tender);

        var derivedAcceptedTenders = new List<TransportationTender>(currentState.CompanyState.AcceptedTenders) { tender };


        var derivedSimulationState = currentState.SimulationState with { OpenTenders = derivedOpenTenders };
        var derivedCompanyState = currentState.CompanyState with
        {
            AcceptedTenders = derivedAcceptedTenders,
        };

        return currentState with
        {
            SimulationState = derivedSimulationState,
            CompanyState = derivedCompanyState,
        };
    }
}