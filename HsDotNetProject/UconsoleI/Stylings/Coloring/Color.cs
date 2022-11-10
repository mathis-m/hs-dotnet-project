using System.Dynamic;

namespace UconsoleI.Stylings.Coloring;

public readonly record struct Color(byte Red, byte Green, byte Blue)
{
    public Color(int hex) : this((byte) ((hex & 0xff0000) >> 16), (byte) ((hex & 0xff00) >> 8), (byte) (hex & 0xff))
    {
    }

    public static Color FromConsoleColor(ConsoleColor color)
    {
        return color switch
        {
            ConsoleColor.Black => DefaultColors.Black,
            ConsoleColor.Blue => DefaultColors.Blue,
            ConsoleColor.Cyan => DefaultColors.Aqua,
            ConsoleColor.DarkBlue => DefaultColors.Navy,
            ConsoleColor.DarkCyan => DefaultColors.Teal,
            ConsoleColor.DarkGray => DefaultColors.Grey,
            ConsoleColor.DarkGreen => DefaultColors.Green,
            ConsoleColor.DarkMagenta => DefaultColors.Purple,
            ConsoleColor.DarkRed => DefaultColors.Maroon,
            ConsoleColor.DarkYellow => DefaultColors.Olive,
            ConsoleColor.Gray => DefaultColors.Silver,
            ConsoleColor.Green => DefaultColors.Lime,
            ConsoleColor.Magenta => DefaultColors.Fuchsia,
            ConsoleColor.Red => DefaultColors.Red,
            ConsoleColor.White => DefaultColors.White,
            ConsoleColor.Yellow => DefaultColors.Yellow,
            _ => DefaultColors.Black,
        };
    }

    public int? ConsoleColorColor {
        get
        {

            return this switch
            {
                var x when x == DefaultColors.Black => (int)ConsoleColor.Black,
                var x when x == DefaultColors.Blue => (int)ConsoleColor.Blue,
                var x when x == DefaultColors.Aqua => (int)ConsoleColor.Cyan,
                var x when x == DefaultColors.Navy => (int)ConsoleColor.DarkBlue,
                var x when x == DefaultColors.Teal => (int)ConsoleColor.DarkCyan,
                var x when x == DefaultColors.Grey => (int)ConsoleColor.DarkGray,
                var x when x == DefaultColors.Green => (int)ConsoleColor.DarkGreen,
                var x when x == DefaultColors.Purple => (int)ConsoleColor.DarkMagenta,
                var x when x == DefaultColors.Maroon => (int)ConsoleColor.DarkRed,
                var x when x == DefaultColors.Olive => (int)ConsoleColor.DarkYellow,
                var x when x == DefaultColors.Silver => (int)ConsoleColor.Gray,
                var x when x == DefaultColors.Lime => (int)ConsoleColor.Green,
                var x when x == DefaultColors.Fuchsia => (int)ConsoleColor.Magenta,
                var x when x == DefaultColors.Red => (int)ConsoleColor.Red,
                var x when x == DefaultColors.White => (int)ConsoleColor.White,
                var x when x == DefaultColors.Yellow => (int)ConsoleColor.Yellow,
                _ => null,
            };
        }
    }
}