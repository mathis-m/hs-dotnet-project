namespace Common.ConsoleUtils;

public class UserInput : IUserInput
{
    public string PromptForString(string prompt)
    {
        Console.Write($"{prompt}: ");
        var input = Console.ReadLine();
        if (input == null) throw new Exception("User input can not be null");
        return input;
    }
}