using CompanySimulator.State.Reducers;
using FreightMarket.Models;

namespace CompanySimulator.State.Actions;

public record AcceptTenderAction(TransportationTender Payload) : ActionWithPayload<TransportationTender>(Payload, typeof(TenderAcceptedReducer));