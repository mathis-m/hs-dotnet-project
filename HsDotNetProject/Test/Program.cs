// Create a table

using UconsoleI.Components.TableComponent;
using UconsoleI.Components.TableComponent.Borders;
using UconsoleI.Components.TextComponent;
using UconsoleI.Extensions;
using UconsoleI.Rendering;
using UconsoleI.Stylings;
using UconsoleI.Stylings.Coloring;
using UconsoleI.UI;

var table = new Table
{
    Border = TableBorderTypes.Square,
};


// Add some columns test
table.AddColumn(new TableColumn(new Text("#")).Padding(new Padding(0, 1)));
table.AddColumn(new TableColumn(new Text("Ware", new Styling(decoration:Decoration.Bold | Decoration.SlowBlink))).Padding(new Padding(0, 1)));
table.AddColumn(new TableColumn(new Text("Typ")).Padding(new Padding(0, 1)));
table.AddColumn(new TableColumn(new Text("Startort")).Padding(new Padding(0, 1)));
table.AddColumn(new TableColumn(new Text("Zielort")).Padding(new Padding(0, 1)));
table.AddColumn(new TableColumn(new Text("Gewicht")).Padding(new Padding(0, 1)));
table.AddColumn(new TableColumn(new Text("Lieferdatum")).Padding(new Padding(0, 1)));
table.AddColumn(new TableColumn(new Text("Vergütung")).Padding(new Padding(0, 1)));
table.AddColumn(new TableColumn(new Text("Strafe")).Padding(new Padding(0, 1)));

var pad = new Padding(1);
// Add some rows
table.AddRow(
    new Text("1").LeftAligned(),
    new Text("Baumaterialien"),
    new Text("Pritschenwagen"),
    new Text("Berlin"),
    new Text("Rom"),
    new Text("2 T"),
    new Text("17.09.2022"),
    new Text("383 EUR"),
    new Text("260 EUR")
);
table.AddRow(
    new Text("2").LeftAligned(),
    new Text("Baumaterialien"),
    new Text("Pritschenwagen"),
    new Text("Berlin"),
    new Text("Rom"),
    new Text("2 T"),
    new Text("17.09.2022"),
    new Text("383 EUR"),
    new Text("260 EUR")
);
table.AddRow(
    new Text("3").LeftAligned(),
    new Text("Baumaterialien"),
    new Text("Pritschenwagen"),
    new Text("Berlin"),
    new Text("Rom"),
    new Text("2 T"),
    new Text("17.09.2022"),
    new Text("383 EUR"),
    new Text("260 EUR")
);


ConsoleUI.Write(table);