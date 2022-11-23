using UconsoleI.Components.TableComponent.Borders;

namespace CompanySimulator.UI.Borders;

public class VerticalTableBorders : TableBorder
{
    public override string GetPart(TableBorderPart part)
    {
        return part switch
        {
            TableBorderPart.HeaderTopLeft => "",
            TableBorderPart.HeaderTop => "",
            TableBorderPart.HeaderTopSeparator => "",
            TableBorderPart.HeaderTopRight => "",
            TableBorderPart.HeaderLeft => "│",
            TableBorderPart.HeaderSeparator => "│",
            TableBorderPart.HeaderRight => "│",
            TableBorderPart.HeaderBottomLeft => "│",
            TableBorderPart.HeaderBottom => "",
            TableBorderPart.HeaderBottomSeparator => "│",
            TableBorderPart.HeaderBottomRight => "│",
            TableBorderPart.CellLeft => "│",
            TableBorderPart.CellSeparator => "│",
            TableBorderPart.CellRight => "│",
            TableBorderPart.FooterTopLeft => "",
            TableBorderPart.FooterTop => "",
            TableBorderPart.FooterTopSeparator => "",
            TableBorderPart.FooterTopRight => "",
            TableBorderPart.FooterBottomLeft => "",
            TableBorderPart.FooterBottom => "",
            TableBorderPart.FooterBottomSeparator => "",
            TableBorderPart.FooterBottomRight => "",
            _ => throw new InvalidOperationException("Unknown border part."),
        };
    }
}