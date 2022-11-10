namespace Common.Services;

public interface IRandomizerService<TType>
{
    Task<TType> NextAsync();
}