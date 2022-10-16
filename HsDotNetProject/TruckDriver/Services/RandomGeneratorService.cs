namespace TruckDriver.Services;

public abstract class RandomGeneratorService<TType> : IGeneratorService<TType>
{
    private readonly Random _random = new();
    public abstract Task<TType> GenerateAsync();

    protected TType GetRandomItem(IEnumerable<TType> enumerable)
    {
        var list  = enumerable.ToArray();
        var index = _random.Next(list.Length);
        return list[index];
    }
}