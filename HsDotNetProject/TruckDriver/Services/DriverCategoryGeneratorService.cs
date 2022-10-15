using Microsoft.Extensions.Options;
using TruckDriver.Models;
using TruckDriver.Options;

namespace TruckDriver.Services;

public class DriverCategoryGeneratorService : IGeneratorService<DriverCategory>
{
    private readonly DriverCategoryConfig _config;
    private readonly Random _random = new();

    public DriverCategoryGeneratorService(IOptions<DriverCategoryConfig> options)
    {
        _config = options.Value;
    }

    public Task<List<DriverCategory>> GenerateAsync(int count)
    {
        var categories = new List<DriverCategory>();
        for (var i = 0; i < count; i++)
        {
            var driverCategory = GetRandomDriverCategory();
            categories.Add(driverCategory);
        }

        return Task.FromResult(categories);
    }

    private DriverCategory GetRandomDriverCategory()
    {
        var typeIndex = _random.Next(_config.AvailableTypes.Count);
        var type = _config.AvailableTypes[typeIndex];
        var driverCategory = new DriverCategory(type);
        return driverCategory;
    }
}