namespace TruckDriver.Services;

public interface IGeneratorService<TType>
{
    Task<List<TType>> GenerateAsync(int count);
}