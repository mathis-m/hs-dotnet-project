using TruckDriver.Models;
using TruckDriver.Repositories;

namespace TruckDriver.Services;

public class NameRandomizerService : BaseRandomizerService<Name>
{
    private readonly HashSet<Name>   _nameCache = new();
    private readonly INameRepository _repository;

    public NameRandomizerService(INameRepository repository)
    {
        _repository = repository;
    }

    public override async Task<Name> NextAsync()
    {
        var allNames    = (await _repository.GetAllAsync()).ToList();
        var unusedNames = GetUnusedNames(allNames).ToList();

        var hasUniqueNamesLeft = unusedNames.Any();
        if (!hasUniqueNamesLeft)
        {
            unusedNames = allNames;
            _nameCache.Clear();
        }

        var randomName = GetRandomItem(unusedNames);
        _nameCache.Add(randomName);

        return randomName;
    }

    private IEnumerable<Name> GetUnusedNames(IEnumerable<Name> allNames)
    {
        return allNames
            .Where(name => !_nameCache.Contains(name));
    }
}