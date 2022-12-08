using Common.Models;
using CompanySimulator.Models;
using CompanySimulator.State;
using CompanySimulator.State.Reducers;
using FreightMarket.Models;
using NUnit.Framework;
using TruckDriver.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.Tests;

public class HireDriverTests
{
    private const double InitialBalance = 50000;

    private readonly TruckOperator _driverToHire = new(
        new Name("Some", "Name"),
        new SalaryExpectation(1, new Currency("x", "x")),
        new DriverCategory("some cat")
    );

    private RootState State { get; set; } = null!;
    private DriverHiredReducer DriverHiredReducer { get; } = new();

    [SetUp]
    public void Setup()
    {
        State = new RootState(
            new SimulationState(
                DateTime.Now,
                new List<TransportationTender>(),
                new List<TruckOperator> { _driverToHire },
                new List<Truck>()
            ),
            new CompanyState(
                "Test",
                new AccountBalance(InitialBalance, new Currency("EUR", "€")),
                new List<Truck>(),
                new List<TruckOperator>(),
                new List<TransportationTender>(),
                new Dictionary<TruckOperator, Truck>(),
                new Dictionary<Truck, RelocationRequest>(),
                new Dictionary<TransportationTender, Truck>()
            ),
            new ApplicationState(
                Pages.CompanyNamePrompter
            )
        );
    }

    [Test]
    public void ReducingState_WithDriverHiredReducer_AddsDriverToCompanyState()
    {
        // act
        var newState = DriverHiredReducer.Reduce(State, _driverToHire);

        // assert
        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.CompanyState.EmployeeCount, Is.EqualTo(1));
        Assert.That(newState.CompanyState.Employees.Count, Is.EqualTo(1));
        Assert.That(newState.CompanyState.Employees.First(), Is.EqualTo(_driverToHire));
    }

    [Test]
    public void ReducingState_WithDriverHiredReducer_RemovesDriverFromSimulationState()
    {
        // act
        var newState = DriverHiredReducer.Reduce(State, _driverToHire);

        // assert
        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.SimulationState.JobSeekingTruckDrivers.Count, Is.EqualTo(0));
    }
}