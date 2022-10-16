namespace TruckDriver.Services;

public interface IRandomizerService<TType>
{
    Task<TType> NextAsync();
}