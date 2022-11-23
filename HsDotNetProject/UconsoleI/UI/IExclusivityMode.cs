namespace UconsoleI.UI;

public interface IExclusivityMode
{
    T Run<T>(Func<T> func);
    Task<T> RunAsync<T>(Func<Task<T>> func);
}