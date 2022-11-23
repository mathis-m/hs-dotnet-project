using System.Globalization;
using UconsoleI.Components.TextComponent;
using UconsoleI.Stylings;
using UconsoleI.UI;

namespace UconsoleI.Extensions;

public static class ConsoleUIExtensions
{
    public static T RunExclusive<T>(this IConsoleUI console, Func<T> func)
    {
        return console.ExclusivityMode.Run(func);
    }

    public static Task<T> RunExclusive<T>(this IConsoleUI console, Func<Task<T>> func)
    {
        return console.ExclusivityMode.RunAsync(func);
    }

    public static void WriteLine(this IConsoleUI console)
    {
        if (console is null) throw new ArgumentNullException(nameof(console));

        console.Write(Text.NewLine);
    }

    public static void WriteLine(this IConsoleUI console, string text, Styling? style = null)
    {
        if (console is null) throw new ArgumentNullException(nameof(console));

        if (text is null) throw new ArgumentNullException(nameof(text));

        console.Write(text + Environment.NewLine, style);
    }

    internal static async Task<string> ReadLine(this IConsoleUI console, Styling? style, IEnumerable<string>? items = null, CancellationToken cancellationToken = default)
    {
        if (console is null) throw new ArgumentNullException(nameof(console));

        style ??= DefaultStylings.Plain;
        var text = string.Empty;

        var autocomplete = new List<string>(items ?? Enumerable.Empty<string>());

        while (true)
        {
            var rawKey = await console.Input.ReadKeyAsync(true, cancellationToken).ConfigureAwait(false);
            if (rawKey == null) continue;

            var key = rawKey.Value;
            if (key.Key == ConsoleKey.Enter) return text;

            if (key.Key == ConsoleKey.Tab && autocomplete.Count > 0)
            {
                var autoCompleteDirection = key.Modifiers.HasFlag(ConsoleModifiers.Shift)
                    ? AutoCompleteDirection.Backward
                    : AutoCompleteDirection.Forward;
                var replace = AutoComplete(autocomplete, text, autoCompleteDirection);
                if (!string.IsNullOrEmpty(replace))
                {
                    // Render the suggestion
                    console.Write(string.Concat("\b \b".Repeat(text.Length)), style);
                    console.Write(replace);
                    text = replace;
                    continue;
                }
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (text.Length > 0)
                {
                    text = text.Substring(0, text.Length - 1);
                    console.Write("\b \b");
                }

                continue;
            }

            if (char.IsControl(key.KeyChar)) continue;

            text += key.KeyChar.ToString();
            var output = key.KeyChar.ToString();
            console.Write(output, style);
        }
    }

    private static string AutoComplete(List<string> autocomplete, string text, AutoCompleteDirection autoCompleteDirection)
    {
        var found   = autocomplete.Find(i => i == text);
        var replace = string.Empty;

        if (found == null)
        {
            // Get the closest match
            var next = autocomplete.Find(i => i.StartsWith(text, true, CultureInfo.InvariantCulture));
            if (next != null)
                replace = next;
            else if (string.IsNullOrEmpty(text))
                // Use the first item
                replace = autocomplete[0];
        }
        else
        {
            // Get the next match
            replace = GetAutocompleteValue(autoCompleteDirection, autocomplete, found);
        }

        return replace;
    }

    private static string GetAutocompleteValue(AutoCompleteDirection autoCompleteDirection, IList<string> autocomplete, string found)
    {
        var foundAutocompleteIndex = autocomplete.IndexOf(found);
        var index = autoCompleteDirection switch
        {
            AutoCompleteDirection.Forward => foundAutocompleteIndex + 1,
            AutoCompleteDirection.Backward => foundAutocompleteIndex - 1,
            _ => throw new ArgumentOutOfRangeException(nameof(autoCompleteDirection), autoCompleteDirection, null),
        };

        if (index >= autocomplete.Count) index = 0;

        if (index < 0) index = autocomplete.Count - 1;

        return autocomplete[index];
    }

    private enum AutoCompleteDirection
    {
        Forward,
        Backward,
    }
}