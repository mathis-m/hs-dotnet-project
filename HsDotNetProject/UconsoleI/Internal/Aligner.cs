using UconsoleI.Elements;
using UconsoleI.Extensions;
using UconsoleI.Rendering;

namespace UconsoleI.Internal;

internal static class Aligner
{
    public static string Align(string text, Justify? alignment, int maxWidth)
    {
        if (alignment is null or Justify.Left) return text;

        var width = CellUtils.GetCellLength(text);
        if (width >= maxWidth) return text;

        switch (alignment)
        {
            case Justify.Right:
            {
                var diff = maxWidth - width;
                return new string(' ', diff) + text;
            }

            case Justify.Center:
            {
                var diff = (maxWidth - width) / 2;

                var left  = new string(' ', diff);
                var right = new string(' ', diff);

                // Right side
                var remainder             = (maxWidth - width) % 2;
                if (remainder != 0) right += new string(' ', remainder);

                return left + text + right;
            }

            default:
                throw new NotSupportedException("Unknown alignment");
        }
    }

    public static void Align<T>(T elements, Justify? alignment, int maxWidth)
        where T : List<Element>
    {
        if (alignment == null || alignment == Justify.Left) return;

        var width = elements.CellCount();
        if (width >= maxWidth) return;

        switch (alignment)
        {
            case Justify.Right:
            {
                var diff = maxWidth - width;
                elements.Insert(0, DefaultElements.Padding(diff));
                break;
            }

            case Justify.Center:
            {
                // Left side.
                var diff = (maxWidth - width) / 2;
                elements.Insert(0, DefaultElements.Padding(diff));

                // Right side
                elements.Add(DefaultElements.Padding(diff));
                var remainder = (maxWidth - width) % 2;
                if (remainder != 0) elements.Add(DefaultElements.Padding(remainder));

                break;
            }

            default:
                throw new NotSupportedException("Unknown alignment");
        }
    }
}