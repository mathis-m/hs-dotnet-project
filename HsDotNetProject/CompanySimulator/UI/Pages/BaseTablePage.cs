using CompanySimulator.State;
using CompanySimulator.State.Actions;
using UconsoleI.Components.TableComponent;
using UconsoleI.Extensions;
using UconsoleI.Prompt;
using UconsoleI.Rendering;
using UconsoleI.UI;

namespace CompanySimulator.UI.Pages;

public abstract class BaseTablePage<T> : IPage, IStateListener
{
    private readonly StateManager _stateManager;

    protected BaseTablePage(StateManager stateManager)
    {
        _stateManager = stateManager;
        _stateManager.SubscribeToStateChanges(this);
    }

    protected abstract string Prompt { get; }
    protected abstract List<TableColumn> TableColumns { get; }

    private IReadOnlyList<T> Options => GetSimulationList(_stateManager.CurrentState);

    private IEnumerable<string> NumChoices => Options.Count > 0
        ? Enumerable.Range(1, Options.Count).Select(x => $"{x}")
        : new List<string>();

    private TextPrompt<string> ChoicePrompt => new TextPrompt<string>(
            Options.Count > 0
                ? $"{Prompt} using 1-{Options.Count} or go to main menu with z"
                : "Go back to main menu with z"
        )
        {
            Choices = NumChoices.Concat(new[] { "z" }).ToList(),
        }
        .ShowChoices(false);

    public void Render()
    {
        ConsoleUI.Write(GetTable());
        var selectedOptionOrZ = ConsoleUI.Prompt(ChoicePrompt);
        if (selectedOptionOrZ != "z")
        {
            var selectedNum    = int.Parse(selectedOptionOrZ);
            var selectedOption = Options[selectedNum - 1];

            _stateManager.DispatchAction(GetOnSelectedAction(selectedOption));
        }

        _stateManager.DispatchAction(new DisplayPageAction(State.Pages.MainMenu));
    }

    public void OnStateChanged(RootState oldState, RootState newState)
    {
        // This is expected its a immutable list.
        // If the reference changes, we know that the values are different.
        // No need to compare the values.
        // ReSharper disable once PossibleUnintendedReferenceComparison
        if (GetSimulationList(oldState) == GetSimulationList(newState)) return;

        ConsoleUI.Console.Clear();
        Render();
    }

    protected abstract IReadOnlyList<T> GetSimulationList(RootState state);
    protected abstract ActionWithPayload<T> GetOnSelectedAction(T item);
    protected abstract IEnumerable<IComponent> GetTableRow(int num, T item);

    private Table GetTable()
    {
        var table = SetupTable();
        AddOptionsToTable(table);
        return table;
    }

    private Table SetupTable()
    {
        var padding = new Padding(0, 1);

        var table = new Table
        {
            Expand = true,
        };

        foreach (var column in TableColumns) table.AddColumn(column.Padding(padding));

        return table;
    }

    private void AddOptionsToTable(Table table)
    {
        var num = 1;
        foreach (var item in Options)
        {
            table.AddRow(GetTableRow(num, item));
            num++;
        }
    }
}