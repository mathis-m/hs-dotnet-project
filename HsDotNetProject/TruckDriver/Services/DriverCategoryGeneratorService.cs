using TruckDriver.Models;
using TruckDriver.Repositories;

namespace TruckDriver.Services;

public class DriverCategoryGeneratorService : RandomGeneratorService<DriverCategory>
{
    private readonly IDriverCategoryRepository _repository;

    public DriverCategoryGeneratorService(IDriverCategoryRepository repository)
    {
        _repository = repository;
    }

    public override async Task<DriverCategory> GenerateAsync()
    {
        var allCategories = await _repository.GetAllAsync();
        return GetRandomItem(allCategories);
    }
}