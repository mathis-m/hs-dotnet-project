using UconsoleI.UI;

namespace UconsoleI.Prompt;

public interface IPrompt<T>
{
    T Show(IConsoleUI console);
    Task<T> ShowAsync(IConsoleUI console, CancellationToken cancellationToken);
}