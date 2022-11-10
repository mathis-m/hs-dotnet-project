using Common.Services;
using TruckDriver.Models;
using TruckDriver.Repositories;

namespace TruckDriver.Services;

public class DriverCategoryRandomizerService : BaseRandomizerService<DriverCategory>
{
    private readonly IDriverCategoryRepository _repository;

    public DriverCategoryRandomizerService(IDriverCategoryRepository repository)
    {
        _repository = repository;
    }

    public override async Task<DriverCategory> NextAsync()
    {
        var allCategories = await _repository.GetAllAsync();
        return GetRandomItem(allCategories);
    }
}