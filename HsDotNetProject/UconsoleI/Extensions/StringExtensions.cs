using System.Text;
using UconsoleI.Internal;
using UconsoleI.Internal.Text;
using Wcwidth;

namespace UconsoleI.Extensions;

internal static class StringExtensions
{
    private static readonly bool AlreadyNormalized
        = Environment.NewLine.Equals("\n", StringComparison.OrdinalIgnoreCase);

    internal static string NormalizeNewLines(this string? text, bool native = false)
    {
        text =   text?.Replace("\r\n", "\n");
        text ??= string.Empty;

        if (native && !AlreadyNormalized)
        {
            text = text.Replace("\n", Environment.NewLine);
        }

        return text;
    }

    internal static string[] SplitLines(this string text)
    {
        var result = text?.NormalizeNewLines()?.Split(new[] { '\n' }, StringSplitOptions.None);
        return result ?? Array.Empty<string>();
    }

    internal static IEnumerable<string> SplitOnMaxWidth(this string text, int maxWidth)
    {
        var list = new List<string>();

        var length = 0;
        var sb     = new StringBuilder();
        foreach (var ch in text)
        {
            if (length + UnicodeCalculator.GetWidth(ch) > maxWidth)
            {
                list.Add(sb.ToString());
                sb.Clear();
                length = 0;
            }

            length += UnicodeCalculator.GetWidth(ch);
            sb.Append(ch);
        }

        list.Add(sb.ToString());

        return list;
    }

    internal static IEnumerable<string> SplitWords(this string word, StringSplitOptions options = StringSplitOptions.None)
    {
        var result = new List<string>();

        static string Read(StringBuffer reader, Func<char, bool> criteria)
        {
            var buffer = new StringBuilder();
            while (!reader.Eof)
            {
                var current = reader.Peek();
                if (!criteria(current))
                {
                    break;
                }

                buffer.Append(reader.Read());
            }

            return buffer.ToString();
        }

        using (var reader = new StringBuffer(word))
        {
            while (!reader.Eof)
            {
                var current = reader.Peek();
                if (char.IsWhiteSpace(current))
                {
                    var x = Read(reader, char.IsWhiteSpace);
                    if (options != StringSplitOptions.RemoveEmptyEntries)
                    {
                        result.Add(x);
                    }
                }
                else
                {
                    result.Add(Read(reader, c => !char.IsWhiteSpace(c)));
                }
            }
        }

        return result;
    }

    public static int GetCellWidth(this string text)
    {
        return CellUtils.GetCellLength(text);
    }
}