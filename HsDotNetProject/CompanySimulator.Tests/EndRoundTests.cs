using Common.Models;
using CompanySimulator.Models;
using CompanySimulator.State;
using CompanySimulator.State.Reducers;
using FreightMarket.Models;
using NUnit.Framework;
using TruckDriver.Models;
using VehicleAcquisition.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.Tests;

public class EndRoundTests
{
    private const double InitialBalance = 50000;

    private static readonly Currency Currency = new("x", "x");

    private static readonly Location InitialLocation = new("Esslingen");

    private readonly TruckOperator _driver = new(
        new Name("Some", "Name"),
        new SalaryExpectation(1, new Currency("x", "x")),
        new DriverCategory(DriverCategory.LovesHisJobType)
    );

    private readonly DateTime _nextDate = new(2020, 2, 3);


    private readonly DateTime _startDate = new(2020, 2, 2);

    private readonly Truck _truck = new TankerTruck(Size.ExtraLarge, new Age(5), InitialLocation);

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
                new List<Truck> { _truck },
                new List<TruckOperator> { _driver },
                new List<TransportationTender>(),
                new Dictionary<TruckOperator, Truck> { { _driver, _truck } },
                new Dictionary<Truck, RelocationRequest>(),
                new Dictionary<TransportationTender, Truck>()
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

    [Test]
    public void ReducingState_WithRoundEndedReducer_UpdatesRelocationStatus_IfArrivalDateReached()
    {
        //prepare
        var request = new RelocationRequest(new Location("Rom"), RelocationStatus.RelocationStarted, _nextDate, new RelocationStats(10, 10, 80, TimeSpan.FromHours(8)));

        var state = State with
        {
            CompanyState = State.CompanyState with
            {
                TruckRelocationRequests = new Dictionary<Truck, RelocationRequest>
                {
                    { _truck, request },
                },
            },
        };

        // act
        var newState = RoundEndedReducer.Reduce(state);

        // assert
        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.CompanyState.TruckRelocationRequests.First().Value.Status, Is.EqualTo(RelocationStatus.Arrived));
    }

    [Test]
    public void ReducingState_WithRoundEndedReducer_UpdatesTruckLocation_IfArrivalDateReached()
    {
        //prepare
        var targetLocation = new Location("Rom");
        var request        = new RelocationRequest(targetLocation, RelocationStatus.RelocationStarted, _nextDate, new RelocationStats(10, 10, 80, TimeSpan.FromHours(8)));

        var state = State with
        {
            CompanyState = State.CompanyState with
            {
                TruckRelocationRequests = new Dictionary<Truck, RelocationRequest>
                {
                    { _truck, request },
                },
            },
        };

        // act
        var newState = RoundEndedReducer.Reduce(state);

        // assert
        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.CompanyState.OwnedTrucks.First().Location, Is.EqualTo(targetLocation));
    }

    [Test]
    public void ReducingState_WithRoundEndedReducer_DoesNotUpdateTruckLocation_IfArrivalDateNotReached()
    {
        //prepare
        var targetLocation = new Location("Rom");
        var request        = new RelocationRequest(targetLocation, RelocationStatus.RelocationStarted, _nextDate, new RelocationStats(1, 10, 80, TimeSpan.FromHours(80)));

        var state = State with
        {
            CompanyState = State.CompanyState with
            {
                TruckRelocationRequests = new Dictionary<Truck, RelocationRequest>
                {
                    { _truck, request },
                },
            },
        };

        // act
        var newState = RoundEndedReducer.Reduce(state);

        // assert
        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.CompanyState.OwnedTrucks.First().Location, Is.EqualTo(InitialLocation));
    }

    [Test]
    public void ReducingState_WithRoundEndedReducer_UpdatesAccountBalance_IfArrivalDateReached()
    {
        //prepare
        var targetLocation = new Location("Rom");
        var request        = new RelocationRequest(targetLocation, RelocationStatus.RelocationStarted, _nextDate, new RelocationStats(10, 10, 80, TimeSpan.FromHours(8)));

        var state = State with
        {
            CompanyState = State.CompanyState with
            {
                TruckRelocationRequests = new Dictionary<Truck, RelocationRequest>
                {
                    { _truck, request },
                },
            },
        };

        // act
        var newState = RoundEndedReducer.Reduce(state);

        // assert
        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.CompanyState.AccountBalance.Balance, Is.LessThan(InitialBalance));
    }
}