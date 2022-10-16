using TruckDriver.Models;
using TruckDriver.Providers;

namespace TruckDriver.Repositories;

public class NameRepository : INameRepository
{
    private readonly INameProvider _nameProvider;

    public NameRepository(INameProvider nameProvider)
    {
        _nameProvider = nameProvider;
    }

    public IEnumerable<Name> GetAll()
    {
        var names = _nameProvider.GetAll().ToList();
        var namesGeneratedByCrossProduct = (
            from nameForFirst in names
            from nameForLast in names
            select new Name(nameForFirst.FirstName, nameForLast.LastName)
        ).ToList();

        return namesGeneratedByCrossProduct;
    }

    public async Task<IEnumerable<Name>> GetAllAsync()
    {
        var names = (await _nameProvider.GetAllAsync()).ToList();
        var namesGeneratedByCrossProduct = (
            from nameForFirst in names
            from nameForLast in names
            select new Name(nameForFirst.FirstName, nameForLast.LastName)
        ).ToList();

        return namesGeneratedByCrossProduct;
    }

    public Name GetById(string fullName)
    {
        var allNames = GetAll();
        return FindNameByFullName(fullName, allNames);
    }

    public async Task<Name> GetByIdAsync(string fullName)
    {
        var allNames = await GetAllAsync();
        return FindNameByFullName(fullName, allNames);
    }

    private static Name FindNameByFullName(string fullName, IEnumerable<Name> allNames)
    {
        var nameWithMatchingFullName = allNames
            .FirstOrDefault(x => x.FullName == fullName);

        if (nameWithMatchingFullName == null) throw new InvalidOperationException($"No name with fullname '{fullName}' exists.");

        return nameWithMatchingFullName;
    }
}