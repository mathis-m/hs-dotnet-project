namespace Common.ConsoleUtils;

public interface IUserChoiceInput<TKey> where TKey : notnull
{
    TKey PromptUserChoice(Dictionary<TKey, string> choices, string title = Constants.DefaultChoiceTitle,
        string promptText = Constants.DefaultChoicePrompt);

    List<TKey> PromptUserMultiChoice(Dictionary<TKey, string> choices, int count,
        string title = Constants.DefaultMultiChoiceTitle);

    List<TKey> PromptUserMultiChoice(Dictionary<TKey, string> choices, List<string> promptTexts, string title);
}