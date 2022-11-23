namespace UconsoleI.UI;

public interface IConsoleUIInput
{
    bool IsKeyAvailable();
    ConsoleKeyInfo? ReadKey(bool intercept);
    Task<ConsoleKeyInfo?> ReadKeyAsync(bool intercept, CancellationToken cancellationToken);
}