namespace Common.ConsoleUtils;

public class StringUserInput : IUserInput<string>
{
    public string Prompt(string prompt)
    {
        Console.Write($"{prompt}: ");
        var input = Console.ReadLine();
        if (input == null) throw new Exception(Constants.UserInputCanNotBeNull);

        return input;
    }
}