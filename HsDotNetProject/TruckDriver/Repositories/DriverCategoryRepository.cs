using Microsoft.Extensions.Options;
using TruckDriver.Models;
using TruckDriver.Options;

namespace TruckDriver.Repositories;

public class DriverCategoryRepository : IDriverCategoryRepository
{
    private readonly DriverCategoryConfig _config;

    public DriverCategoryRepository(IOptions<DriverCategoryConfig> options)
    {
        _config = options.Value;
    }

    public IEnumerable<DriverCategory> GetAll()
    {
        return _config.AvailableTypes
            .Select(type => new DriverCategory(type));
    }

    public Task<IEnumerable<DriverCategory>> GetAllAsync()
    {
        return Task.FromResult(GetAll());
    }

    public DriverCategory GetById(string type)
    {
        var allCategories = GetAll();
        return GetCategoryWithType(type, allCategories);
    }

    public async Task<DriverCategory> GetByIdAsync(string type)
    {
        var allCategories = await GetAllAsync();
        return GetCategoryWithType(type, allCategories);
    }

    private static DriverCategory GetCategoryWithType(string type, IEnumerable<DriverCategory> allCategories)
    {
        var categoryWithType = allCategories.FirstOrDefault(c => c.Type == type);
        if (categoryWithType == null) throw new InvalidOperationException($"{nameof(DriverCategory)} with type '${type}' does not exist");

        return categoryWithType;
    }
}