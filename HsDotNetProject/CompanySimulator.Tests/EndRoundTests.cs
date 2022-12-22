using Common.BngUtils;
using Common.Models;
using CompanySimulator.Factories;
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

    private readonly Truck _truck = new TankerTruck(Size.ExtraLarge, new Age(5), InitialLocation);

    private readonly TruckOperator _driver = new(
        new Name("Some", "Name"),
        new SalaryExpectation(1, new Currency("x", "x")),
        new DriverCategory(DriverCategory.LovesHisJobType)
    );


    private readonly DateTime _startDate = new(2020, 2, 2);
    private readonly DateTime _nextDate  = new(2020, 2, 3);

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
                }
            }
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
                }
            }
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
                }
            }
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
                }
            }
        };

        // act
        var newState = RoundEndedReducer.Reduce(state);

        // assert
        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.CompanyState.AccountBalance.Balance, Is.LessThan(InitialBalance));
    }
}

public class RelocationStatsFactoryTests
{
    class BngDistanceCalculatorMock : IBngDistanceCalculator
    {
        private readonly double _distance;

        public BngDistanceCalculatorMock(double distance)
        {
            _distance = distance;
        }

        public double CalculateDistanceInMeters(BngPoint start, BngPoint end)
        {
            return _distance;
        }
    }

    private readonly RelocationStatsFactory _defaultSut = new(new BngDistanceCalculatorMock(1000));


    [Test]
    public void RelocationStatsFactory_8Hours_per_day()
    {
        // prepare
        var truck       = new RefrigeratedTruck(Size.Medium, new Age(3), new Location("Esslingen"));
        var driver      = new TruckOperator(new Name("a", "b"), new SalaryExpectation(0, new Currency("EUR", "€")), new DriverCategory(DriverCategory.LovesHisJobType));
        var destination = new Location("Rom");

        const int payloadInT = 0;
        const int avgKmH     = 70;
        const int hours      = 8;

        var sut = new RelocationStatsFactory(new BngDistanceCalculatorMock(avgKmH * 1000 * hours));

        // act
        var stats = sut.Create(truck, driver, destination, payloadInT);

        // assert
        Assert.That(stats, Is.Not.Null);
        Assert.That(stats.DurationInDays.TotalDays, Is.EqualTo(1));
    }


    [Test]
    public void RelocationStatsFactory_AvgSpeedDefault_IfNormalDriverAndNoPayload()
    {
        // prepare
        var truck       = new RefrigeratedTruck(Size.Medium, new Age(3), new Location("Esslingen"));
        var driver      = new TruckOperator(new Name("a", "b"), new SalaryExpectation(0, new Currency("EUR", "€")), new DriverCategory(DriverCategory.LovesHisJobType));
        var destination = new Location("Rom");
        var payloadInT  = 0;

        // act
        var stats = _defaultSut.Create(truck, driver, destination, payloadInT);

        // assert
        Assert.That(stats, Is.Not.Null);
        Assert.That(stats.AvgSpeedInKmH, Is.EqualTo(70));
    }

    [Test]
    public void RelocationStatsFactory_AvgSpeedRacer_IfNormalDriverAndNoPayload()
    {
        // prepare
        var truck       = new RefrigeratedTruck(Size.Medium, new Age(3), new Location("Esslingen"));
        var driver      = new TruckOperator(new Name("a", "b"), new SalaryExpectation(0, new Currency("EUR", "€")), new DriverCategory(DriverCategory.RacerType));
        var destination = new Location("Rom");
        var payloadInT  = 0;

        // act
        var stats = _defaultSut.Create(truck, driver, destination, payloadInT);

        // assert
        Assert.That(stats, Is.Not.Null);
        Assert.That(stats.AvgSpeedInKmH, Is.EqualTo(73));
    }

    [Test]
    public void RelocationStatsFactory_AvgSpeedDreamer_IfNormalDriverAndNoPayload()
    {
        // prepare
        var truck       = new RefrigeratedTruck(Size.Medium, new Age(3), new Location("Esslingen"));
        var driver      = new TruckOperator(new Name("a", "b"), new SalaryExpectation(0, new Currency("EUR", "€")), new DriverCategory(DriverCategory.DreamyType));
        var destination = new Location("Rom");
        var payloadInT  = 0;

        // act
        var stats = _defaultSut.Create(truck, driver, destination, payloadInT);

        // assert
        Assert.That(stats, Is.Not.Null);
        Assert.That(stats.AvgSpeedInKmH, Is.EqualTo(68));
    }

    [Test]
    public void RelocationStatsFactory_AvgSpeedAdjustment_IfNormalDriverAndTooLargePayload()
    {
        // prepare
        var truck       = new RefrigeratedTruck(Size.Medium, new Age(3), new Location("Esslingen"));
        var driver      = new TruckOperator(new Name("a", "b"), new SalaryExpectation(0, new Currency("EUR", "€")), new DriverCategory(DriverCategory.LovesHisJobType));
        var destination = new Location("Rom");
        var payloadInT  = (truck.EnginePowerInKw + 10) / 7.5;

        // act
        var stats = _defaultSut.Create(truck, driver, destination, payloadInT);

        // assert
        Assert.That(stats, Is.Not.Null);
        Assert.That(stats.AvgSpeedInKmH, Is.LessThan(70));
    }

    [Test]
    public void RelocationStatsFactory_FuelConsumption_DefaultsToTruckConsumption()
    {
        // prepare
        var truck       = new RefrigeratedTruck(Size.Medium, new Age(3), new Location("Esslingen"));
        var driver      = new TruckOperator(new Name("a", "b"), new SalaryExpectation(0, new Currency("EUR", "€")), new DriverCategory(DriverCategory.LovesHisJobType));
        var destination = new Location("Rom");
        var payloadInT  = 0;

        // act
        var stats = _defaultSut.Create(truck, driver, destination, payloadInT);

        // assert
        Assert.That(stats, Is.Not.Null);
        Assert.That(stats.ConsumptionPer100Km, Is.EqualTo(truck.ConsumptionPer100KmInL));
    }

    [Test]
    public void RelocationStatsFactory_FuelConsumption_AdjustedWhenRacer()
    {
        // prepare
        var truck       = new RefrigeratedTruck(Size.Medium, new Age(3), new Location("Esslingen"));
        var driver      = new TruckOperator(new Name("a", "b"), new SalaryExpectation(0, new Currency("EUR", "€")), new DriverCategory(DriverCategory.RacerType));
        var destination = new Location("Rom");
        var payloadInT  = 0;

        // act
        var stats = _defaultSut.Create(truck, driver, destination, payloadInT);

        // assert
        Assert.That(stats, Is.Not.Null);
        Assert.That(stats.ConsumptionPer100Km, Is.GreaterThan(truck.ConsumptionPer100KmInL));
    }
}