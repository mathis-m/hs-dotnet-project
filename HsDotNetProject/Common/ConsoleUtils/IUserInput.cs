namespace Common.ConsoleUtils;

public interface IUserInput<out TInput> where TInput : notnull
{
    TInput Prompt(string prompt);
}