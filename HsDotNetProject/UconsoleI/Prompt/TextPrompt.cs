using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using UconsoleI.Extensions;
using UconsoleI.Internal;
using UconsoleI.Stylings;
using UconsoleI.UI;

namespace UconsoleI.Prompt;

public class TextPrompt<T> : IPrompt<T>
{
    private readonly StringComparer? _comparer;
    private readonly string          _prompt;

    public TextPrompt(string prompt, StringComparer? comparer = null)
    {
        _prompt = prompt ?? throw new ArgumentNullException(nameof(prompt));
    }

    public Func<T, ValidationResult>? Validator { get; set; }
    public CultureInfo? Culture { get; set; }
    public bool AllowEmpty { get; set; }
    public string ValidationErrorMessage { get; set; } = "Invalid input";
    public string InvalidChoiceMessage { get; set; } = "Please select one of the available options";
    public bool ShowDefaultValue { get; set; } = true;
    public bool ShowChoices { get; set; } = true;
    public List<T> Choices { get; init; } = new();
    public Styling PromptStyling { get; set; }
    public Func<T, string>? Converter { get; set; } = TypeConverterHelper.ConvertToString;
    internal DefaultPromptValue<T>? DefaultValue { get; set; }

    public T Show(IConsoleUI console)
    {
        return ShowAsync(console, CancellationToken.None).GetAwaiter().GetResult();
    }

    public async Task<T> ShowAsync(IConsoleUI console, CancellationToken cancellationToken)
    {
        if (console is null) throw new ArgumentNullException(nameof(console));
        return await console.RunExclusive(async () =>
        {
            var promptStyle = PromptStyling ?? DefaultStylings.Plain;
            var converter   = Converter ?? TypeConverterHelper.ConvertToString;
            var choices     = Choices.Select(choice => converter(choice)).ToList();
            var choiceMap   = Choices.ToDictionary(choice => converter(choice), choice => choice, _comparer);

            WritePrompt(console);

            while (true)
            {
                var input = await console.ReadLine(promptStyle, choices, cancellationToken).ConfigureAwait(false);

                // Nothing entered?
                if (string.IsNullOrWhiteSpace(input))
                {
                    if (DefaultValue != null)
                    {
                        var defaultValue = converter(DefaultValue.Value);
                        console.Write(defaultValue, promptStyle);
                        console.WriteLine();
                        return DefaultValue.Value;
                    }

                    if (!AllowEmpty) continue;
                }

                console.WriteLine();

                T? result;
                if (Choices.Count > 0)
                {
                    if (choiceMap.TryGetValue(input, out result) && result != null) return result;

                    console.WriteLine(InvalidChoiceMessage);
                    WritePrompt(console);
                    continue;
                }

                if (!TypeConverterHelper.TryConvertFromStringWithCulture(input, Culture, out result) || result == null)
                {
                    console.WriteLine(ValidationErrorMessage);
                    WritePrompt(console);
                    continue;
                }

                if (ValidateResult(result, out var validationMessage)) return result;

                console.WriteLine(validationMessage);
                WritePrompt(console);
            }
        }).ConfigureAwait(false);
    }

    private bool ValidateResult(T value, [NotNullWhen(false)] out string? message)
    {
        if (Validator != null)
        {
            var result = Validator(value);
            if (!result.Successful)
            {
                message = result.Message ?? ValidationErrorMessage;
                return false;
            }
        }

        message = null;
        return true;
    }


    private void WritePrompt(IConsoleUI console)
    {
        if (console is null) throw new ArgumentNullException(nameof(console));

        var builder = new StringBuilder();
        builder.Append(_prompt.TrimEnd());

        var appendSuffix = false;
        if (ShowChoices && Choices.Count > 0)
        {
            appendSuffix = true;
            var converter = Converter ?? TypeConverterHelper.ConvertToString;
            var choices   = string.Join("/", Choices.Select(choice => converter(choice)));
            builder.AppendFormat(CultureInfo.InvariantCulture, " [{0}]", choices);
        }

        if (ShowDefaultValue && DefaultValue != null)
        {
            appendSuffix = true;
            var converter    = Converter ?? TypeConverterHelper.ConvertToString;
            var defaultValue = converter(DefaultValue.Value);

            builder.AppendFormat(
                CultureInfo.InvariantCulture,
                " ({0})",
                defaultValue
            );
        }

        var prompt               = builder.ToString().Trim();
        if (appendSuffix) prompt += ":";

        console.Write(prompt + " ", PromptStyling);
    }
}