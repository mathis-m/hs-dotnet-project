using Common.Extensions;
using TruckDriver.Models;
using TruckDriver.Services;

namespace TruckDriver;

public class TruckOperatorListingApplication
{
    private readonly IRandomizerService<TruckOperator> _truckOperatorRandomizer;

    public TruckOperatorListingApplication(IRandomizerService<TruckOperator> truckOperatorRandomizer)
    {
        _truckOperatorRandomizer = truckOperatorRandomizer;
    }

    public async Task ExecuteAsync()
    {
        var drivers = new List<TruckOperator>();

        for (var i = 0; i < 5; i++) drivers.Add(await _truckOperatorRandomizer.NextAsync());

        drivers.Print(driver =>
            $"{driver.Name.FullName}, {driver.SalaryExpectation.FormattedValueWithIsoCode}, {driver.DriverCategory.Type}"
        );
    }
}