namespace UconsoleI.Extensions;

internal static class TextWriterExtensions
{
    public static bool IsStandardOut(this TextWriter writer)
    {
        try
        {
            return writer == Console.Out;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsStandardError(this TextWriter writer)
    {
        try
        {
            return writer == Console.Error;
        }
        catch
        {
            return false;
        }
    }
}