using TruckDriver.Models;
using TruckDriver.Repositories;

namespace TruckDriver.Services;

public class NameGeneratorService : RandomGeneratorService<Name>
{
    private readonly HashSet<Name>   _nameCache = new();
    private readonly INameRepository _repository;

    public NameGeneratorService(INameRepository repository)
    {
        _repository = repository;
    }

    public override async Task<Name> GenerateAsync()
    {
        var allNames = (await _repository.GetAllAsync()).ToList();
        var unusedNames = allNames
            .Where(name => !_nameCache.Contains(name))
            .ToList();

        var generateUniqueName = unusedNames.Any();
        if (!generateUniqueName)
            return GetRandomItem(allNames);

        var randomName = GetRandomItem(allNames);
        _nameCache.Add(randomName);

        return randomName;
    }
}