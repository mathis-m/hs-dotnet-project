namespace UconsoleI.Components.TableComponent.Borders;

public sealed class NoTableBorder : TableBorder
{
    public override bool Visible => false;

    public override string GetPart(TableBorderPart part)
    {
        return " ";
    }
}