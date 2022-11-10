namespace UconsoleI.Rendering;

public readonly record struct Padding(int Top, int Right, int Bottom, int Left)
{
    public Padding(int size) : this(size, size, size, size)
    {
    }

    public Padding(int vertical, int horizontal) : this(vertical, horizontal, vertical, horizontal)
    {
    }

    public int Width => Left + Right;
    public int Height => Top + Bottom;
}