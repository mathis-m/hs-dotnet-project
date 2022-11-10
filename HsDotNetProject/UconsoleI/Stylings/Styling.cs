﻿using UconsoleI.Stylings.Coloring;

namespace UconsoleI.Stylings;

public record Styling
{
    public Styling(Color? color = null, Color? backgroundColor = null, Decoration? decoration = null)
    {
        this.Color           = color ?? DefaultColors.Color;
        this.BackgroundColor = backgroundColor ?? DefaultColors.BackgroundColor;
        this.Decoration      = decoration ?? Decoration.None;
    }

    public Color Color { get; }
    public Color BackgroundColor { get; }
    public Decoration Decoration { get; }

    public void Deconstruct(out Color color, out Color backgroundColor, out Decoration decoration)
    {
        color           = Color;
        backgroundColor = BackgroundColor;
        decoration      = Decoration;
    }
}