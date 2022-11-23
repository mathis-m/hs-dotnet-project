namespace UconsoleI.UI;

public interface IAnsiConsoleInput
{
    bool IsKeyAvailable();
    ConsoleKeyInfo? ReadKey(bool intercept);
    Task<ConsoleKeyInfo?> ReadKeyAsync(bool intercept, CancellationToken cancellationToken);
}