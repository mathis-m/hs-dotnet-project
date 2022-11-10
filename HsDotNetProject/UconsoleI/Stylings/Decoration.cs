namespace UconsoleI.Stylings;

[Flags]
public enum Decoration
{
    None          = 0,
    Bold          = 1 << 0,
    Dim           = 1 << 1,
    Italic        = 1 << 2,
    Underline     = 1 << 3,
    Invert        = 1 << 4,
    Conceal       = 1 << 5,
    SlowBlink     = 1 << 6,
    RapidBlink    = 1 << 7,
    StrikeThrough = 1 << 8,
}