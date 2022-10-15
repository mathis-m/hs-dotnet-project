using Microsoft.Extensions.Options;
using TruckDriver.Models;
using TruckDriver.Options;

namespace TruckDriver.Providers;

public class NameFromTextFileProvider : INameProvider
{
    private readonly NameFromTextFileProviderConfig _config;

    public NameFromTextFileProvider(IOptions<NameFromTextFileProviderConfig> options)
    {
        _config = options.Value;
    }

    public async Task<IReadOnlyList<Name>> GetAllAsync()
    {
        ThrowIfFileDoesNotExist();

        var rawNames = await File.ReadAllLinesAsync(_config.FilePath);
        return rawNames
            .Select(ParseName)
            .ToList();
    }

    private Name ParseName(string rawName)
    {
        var nameParts = rawName.Split(" ");
        if (nameParts.Length != 2)
            throw new InvalidOperationException(
                $"File with path {_config.FilePath} is in the wrong format. Each line must consist of two words separated by one space.");
        var name = new Name(nameParts[0], nameParts[1]);
        return name;
    }

    private void ThrowIfFileDoesNotExist()
    {
        if (!File.Exists(_config.FilePath))
            throw new InvalidOperationException($"File with path {_config.FilePath} does not exist.");
    }
}