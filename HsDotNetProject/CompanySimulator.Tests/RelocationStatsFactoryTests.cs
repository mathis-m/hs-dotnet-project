using Common.BngUtils;
using Common.Models;
using CompanySimulator.Factories;
using NUnit.Framework;
using TruckDriver.Models;
using VehicleAcquisition.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.Tests;

public class RelocationStatsFactoryTests
{
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

    private class BngDistanceCalculatorMock : IBngDistanceCalculator
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
}