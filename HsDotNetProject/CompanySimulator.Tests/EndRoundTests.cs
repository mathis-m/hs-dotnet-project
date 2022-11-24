using Common.Models;
using CompanySimulator.Models;
using CompanySimulator.State;
using CompanySimulator.State.Reducers;
using FreightMarket.Models;
using NUnit.Framework;
using TruckDriver.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.Tests;

public class EndRoundTests
{
    private const double InitialBalance = 50000;

    private static readonly Currency Currency = new("x", "x");

    private readonly DateTime _startDate = new(2020, 2, 2);

    private RootState State { get; set; } = null!;
    private RoundEndedReducer RoundEndedReducer { get; } = new();

    [SetUp]
    public void Setup()
    {
        State = new RootState(
            new SimulationState(
                _startDate,
                new List<TransportationTender>(),
                new List<TruckOperator>(),
                new List<Truck>()
            ),
            new CompanyState(
                "Test",
                new AccountBalance(InitialBalance, Currency),
                new List<Truck>(),
                new List<TruckOperator>(),
                new List<TransportationTender>()
            ),
            new ApplicationState(
                Pages.CompanyNamePrompter
            )
        );
    }

    [Test]
    public void ReducingState_WithRoundEndedReducer_AddsOneDay()
    {
        // act
        var newState = RoundEndedReducer.Reduce(State);

        // assert
        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.SimulationState.SimulationDate, Is.EqualTo(_startDate.Date.AddDays(1).Date));
    }
}