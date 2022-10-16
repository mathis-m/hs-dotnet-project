namespace TruckDriver.Services;

public interface IGeneratorService<TType>
{
    Task<TType> GenerateAsync();
}