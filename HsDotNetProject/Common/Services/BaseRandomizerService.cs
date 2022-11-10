namespace Common.Services;

public abstract class BaseRandomizerService<TType> : IRandomizerService<TType>
{
    private readonly Random _random = new();
    public abstract Task<TType> NextAsync();

    protected TItem GetRandomItem<TItem>(IEnumerable<TItem> enumerable)
    {
        var list  = enumerable.ToArray();
        var index = _random.Next(list.Length);
        return list[index];
    }
}