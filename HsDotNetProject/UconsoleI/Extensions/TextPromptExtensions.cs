using UconsoleI.Internal;
using UconsoleI.Prompt;

namespace UconsoleI.Extensions;

public static class TextPromptExtensions
{
    public static TextPrompt<T> DefaultValue<T>(this TextPrompt<T> obj, T value)
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        obj.DefaultValue = new DefaultPromptValue<T>(value);
        return obj;
    }

    public static TextPrompt<T> ValidationErrorMessage<T>(this TextPrompt<T> obj, string message)
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        obj.ValidationErrorMessage = message;
        return obj;
    }

    public static TextPrompt<T> Validate<T>(this TextPrompt<T> obj, Func<T, ValidationResult> validator)
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        obj.Validator = validator;

        return obj;
    }

    public static TextPrompt<T> InvalidChoiceMessage<T>(this TextPrompt<T> obj, string message)
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        obj.InvalidChoiceMessage = message;
        return obj;
    }

    public static TextPrompt<T> ShowChoices<T>(this TextPrompt<T> obj, bool show)
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        obj.ShowChoices = show;
        return obj;
    }
}