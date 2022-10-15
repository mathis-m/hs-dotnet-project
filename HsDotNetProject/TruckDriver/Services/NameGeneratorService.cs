using TruckDriver.Models;
using TruckDriver.Providers;

namespace TruckDriver.Services;

public class NameGeneratorService : IGeneratorService<Name>
{
    private readonly INameProvider _nameProvider;
    private readonly Random _random = new();

    public NameGeneratorService(INameProvider nameProvider)
    {
        _nameProvider = nameProvider;
    }

    public async Task<List<Name>> GenerateAsync(int count)
    {
        var names = await _nameProvider.GetAllAsync();
        var generatedNames = GenerateNamesByCrossProduct(names);

        var randomGeneratedNames = new List<Name>();
        for (var i = 0; i < count; i++)
        {
            var randomNameIdx = _random.Next(generatedNames.Count);
            var randomName = generatedNames[randomNameIdx];
            randomGeneratedNames.Add(randomName);
        }

        return randomGeneratedNames;
    }

    private static List<Name> GenerateNamesByCrossProduct(IReadOnlyList<Name> names)
    {
        var namesGeneratedByCrossProduct = (
            from nameForFirst in names
            from nameForLast in names
            select new Name(nameForFirst.FirstName, nameForLast.LastName)
        ).ToList();
        return namesGeneratedByCrossProduct;
    }
}