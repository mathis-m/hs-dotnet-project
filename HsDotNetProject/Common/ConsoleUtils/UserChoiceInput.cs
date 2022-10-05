namespace Common.ConsoleUtils;

public class UserChoiceInput<TKey> : IUserChoiceInput<TKey> where TKey : notnull
{
    private readonly IUserInput<TKey> _userInput;

    public UserChoiceInput(IUserInput<TKey> userInput)
    {
        _userInput = userInput;
    }

    public TKey PromptUserChoice(
        Dictionary<TKey, string> choices,
        string title = Constants.DefaultChoiceTitle,
        string promptText = Constants.DefaultChoicePrompt
    )
    {
        PrintChoices(choices, title);
        var choice = GetSelectedOptionFromUser(choices.Keys, promptText);

        return choice;
    }

    public List<TKey> PromptUserMultiChoice(Dictionary<TKey, string> choices, int count,
        string title = Constants.DefaultMultiChoiceTitle)
    {
        var promptTexts = Enumerable.Range(1, count)
            .Select(idx => $"Choice #{idx}")
            .ToList();

        return PromptUserMultiChoice(choices, promptTexts, title);
    }


    public List<TKey> PromptUserMultiChoice(Dictionary<TKey, string> choices, List<string> promptTexts, string title)
    {
        PrintChoices(choices, title);

        return promptTexts
            .Select(promptText => GetSelectedOptionFromUser(choices.Keys, promptText))
            .ToList();
    }

    private TKey GetSelectedOptionFromUser(IEnumerable<TKey> options, string promptText)
    {
        var enumerable = options.ToList();

        TKey choice;
        bool isInOptionSetIncluded;
        do
        {
            choice = _userInput.Prompt(promptText);
            isInOptionSetIncluded = enumerable
                .Contains(choice);

            if (!isInOptionSetIncluded) Console.WriteLine(Constants.NotInOptionSet);
        } while (!isInOptionSetIncluded);

        return choice;
    }

    private static void PrintChoices(Dictionary<TKey, string> choices, string title)
    {
        Console.WriteLine(title);

        foreach (var (key, text) in choices) Console.WriteLine($"[{key}] {text}");

        Console.WriteLine();
    }
}