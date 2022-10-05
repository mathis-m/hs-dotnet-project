namespace Common.ConsoleUtils;

public class IntUserInput : IUserInput<int>
{
    private readonly IUserInput<string> _stringUserInput;

    public IntUserInput(IUserInput<string> stringUserInput)
    {
        _stringUserInput = stringUserInput;
    }

    public int Prompt(string prompt)
    {
        int parsedInput;
        bool isValidInt;
        do
        {
            var inputRaw = _stringUserInput.Prompt(prompt);
            isValidInt = int.TryParse(inputRaw, out parsedInput);

            if (!isValidInt) Console.WriteLine(Constants.InvalidInt);
        } while (!isValidInt);

        return parsedInput;
    }
}