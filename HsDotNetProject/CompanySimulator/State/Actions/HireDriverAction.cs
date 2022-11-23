using CompanySimulator.State.Reducers;
using TruckDriver.Models;

namespace CompanySimulator.State.Actions;

public record HireDriverAction(TruckOperator Payload) : ActionWithPayload<TruckOperator>(Payload, typeof(DriverHiredReducer));