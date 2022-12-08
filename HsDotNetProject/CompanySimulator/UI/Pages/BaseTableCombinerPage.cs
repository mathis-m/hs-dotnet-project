using CompanySimulator.State;
using CompanySimulator.State.Actions;
using UconsoleI.Components.TableComponent;
using UconsoleI.Extensions;
using UconsoleI.Prompt;
using UconsoleI.Rendering;
using UconsoleI.UI;

namespace CompanySimulator.UI.Pages;

public abstract class BaseTableCombinerPage<TFirst, TSecond, TAction, TActionPayload> : IPage, IStateListener
    where TFirst : class
    where TSecond : class
    where TAction : ActionWithPayload<TActionPayload>
{
    private const string ExitToMainMenuKey = "z";
    private const string StartOverKey      = "r";

    private readonly StateManager _stateManager;

    private TFirst? _firstSelectedOption;

    private TableCombinerState _pageState = TableCombinerState.First;

    protected BaseTableCombinerPage(StateManager stateManager)
    {
        _stateManager = stateManager;
        _stateManager.SubscribeToStateChanges(this);
    }

    protected virtual Action<TFirst>? OnFirstSelected { get; } = null;
    protected virtual Action<TSecond>? OnSecondSelected { get; } = null;

    protected virtual TableTitle? FirstTitle { get; } = null;
    protected virtual TableTitle? SecondTitle { get; } = null;
    protected abstract string FirstPrompt { get; }
    protected abstract string SecondPrompt { get; }
    protected abstract List<TableColumn> FirstTableColumns { get; }
    protected abstract List<TableColumn> SecondTableColumns { get; }

    private IReadOnlyList<TFirst> FirstOptions => GetFirstList(_stateManager.CurrentState);
    private IReadOnlyList<TSecond> SecondOptions => GetSecondList(_stateManager.CurrentState);

    private IEnumerable<string> FirstNumChoices => FirstOptions.Count > 0
        ? Enumerable.Range(1, FirstOptions.Count).Select(x => $"{x}")
        : new List<string>();

    private IEnumerable<string> SecondNumChoices => SecondOptions.Count > 0
        ? Enumerable.Range(1, SecondOptions.Count).Select(x => $"{x}")
        : new List<string>();

    private TextPrompt<string> FirstChoicePrompt => new TextPrompt<string>(
            FirstOptions.Count > 0
                ? $"{FirstPrompt} using 1-{FirstOptions.Count}, go to main menu with z or restart the selection with r"
                : "Go back to main menu with z"
        )
        {
            Choices = FirstNumChoices.Concat(FirstOptions.Count > 0 ? new[] { "z", "r" } : new[] { "z" }).ToList(),
        }
        .ShowChoices(false);

    private TextPrompt<string> SecondChoicePrompt => new TextPrompt<string>(
            SecondOptions.Count > 0
                ? $"{SecondPrompt} using 1-{SecondOptions.Count}, go to main menu with z or restart the selection with r"
                : "Go back to main menu with z or restart the selection with r"
        )
        {
            Choices = SecondNumChoices.Concat(new[] { ExitToMainMenuKey, StartOverKey }).ToList(),
        }
        .ShowChoices(false);

    public void Render()
    {
        switch (_pageState)
        {
            case TableCombinerState.First:
                RenderAndHandleFirstSelection();
                break;
            case TableCombinerState.Second when _firstSelectedOption == null:
                throw new InvalidOperationException("Internal state is not valid");
            case TableCombinerState.Second:
                RenderAndHandleSecondSelection(_firstSelectedOption);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Dispose()
    {
        _stateManager.RemoveListener(this);
    }

    public void OnStateChanged(RootState oldState, RootState newState)
    {
        void ReRender()
        {
            ConsoleUI.Console.Clear();
            Render();
        }

        var firstListChanged   = !Equals(GetFirstList(oldState), GetFirstList(newState));
        var firstListDisplayed = _pageState == TableCombinerState.First;

        var secondListChanged   = !Equals(GetSecondList(oldState), GetSecondList(newState));
        var secondListDisplayed = _pageState == TableCombinerState.First;

        if (firstListChanged && firstListDisplayed) ReRender();
        else if (secondListChanged && secondListDisplayed) ReRender();
    }

    private void RenderAndHandleFirstSelection()
    {
        ConsoleUI.Write(GetFirstTable());
        var (selectedOption, key) = AskUserForSelection(FirstChoicePrompt, FirstOptions);
        if (selectedOption == null)
        {
            HandleExitOrRestart(key);
            return;
        }

        OnFirstSelected?.Invoke(selectedOption);
        _firstSelectedOption = selectedOption;
        _pageState           = TableCombinerState.Second;
        ConsoleUI.Console.Clear();
        Render();
    }

    private void RenderAndHandleSecondSelection(TFirst firstSelection)
    {
        ConsoleUI.Write(GetSecondTable());
        var (selectedOption, key) = AskUserForSelection(SecondChoicePrompt, SecondOptions);
        _pageState                = TableCombinerState.First;
        _firstSelectedOption      = null;
        if (selectedOption == null)
        {
            HandleExitOrRestart(key);
            return;
        }

        OnSecondSelected?.Invoke(selectedOption);

        var action = CreateActionOnBothSelected(firstSelection, selectedOption);
        Dispose();
        _stateManager.DispatchAction(action);
        _stateManager.DispatchAction(new DisplayPageAction(State.Pages.MainMenu));
    }

    private void HandleExitOrRestart(string key)
    {
        switch (key)
        {
            case ExitToMainMenuKey:
                _stateManager.DispatchAction(new DisplayPageAction(State.Pages.MainMenu));
                break;
            case StartOverKey:
                Reset();
                break;
        }
    }

    private void Reset()
    {
        _pageState           = TableCombinerState.First;
        _firstSelectedOption = null;
        ConsoleUI.Console.Clear();
        Render();
    }

    private static (TOption?, string) AskUserForSelection<TOption>(IPrompt<string> prompt, IReadOnlyList<TOption> options)
        where TOption : class
    {
        var selected = ConsoleUI.Prompt(prompt);
        if (selected is ExitToMainMenuKey or StartOverKey) return (null, selected);

        var selectedNum    = int.Parse(selected);
        var selectedOption = options[selectedNum - 1] ?? throw new InvalidOperationException("Internal failure");
        return (selectedOption, selected);
    }

    protected abstract IReadOnlyList<TFirst> GetFirstList(RootState state);
    protected abstract IReadOnlyList<TSecond> GetSecondList(RootState state);
    protected abstract TAction CreateActionOnBothSelected(TFirst firstItem, TSecond secondItem);
    protected abstract IEnumerable<IComponent> GetRowOfFirstTable(int num, TFirst item);
    protected abstract IEnumerable<IComponent> GetRowOfSecondTable(int num, TSecond item);

    private Table GetFirstTable()
    {
        var table                           = SetupTable(FirstTableColumns);
        if (FirstTitle != null) table.Title = FirstTitle;

        AddOptionsToTable(table, FirstOptions, GetRowOfFirstTable);
        return table;
    }

    private Table GetSecondTable()
    {
        var table                            = SetupTable(SecondTableColumns);
        if (SecondTitle != null) table.Title = SecondTitle;

        AddOptionsToTable(table, SecondOptions, GetRowOfSecondTable);
        return table;
    }

    private static Table SetupTable(List<TableColumn> columns)
    {
        var padding = new Padding(0, 1);

        var table = new Table
        {
            Expand = true,
        };

        foreach (var column in columns) table.AddColumn(column.Padding(padding));

        return table;
    }

    private static void AddOptionsToTable<T>(Table table, IEnumerable<T> options, Func<int, T, IEnumerable<IComponent>> getRow)
    {
        var num = 1;
        foreach (var item in options)
        {
            table.AddRow(getRow(num, item));
            num++;
        }
    }

    private enum TableCombinerState
    {
        First,
        Second,
    }
}