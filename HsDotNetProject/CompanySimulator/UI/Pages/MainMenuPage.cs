using CompanySimulator.Models;
using CompanySimulator.State;
using CompanySimulator.State.Actions;
using CompanySimulator.UI.Borders;
using UconsoleI.Components.TableComponent;
using UconsoleI.Extensions;
using UconsoleI.Prompt;
using UconsoleI.Rendering;
using UconsoleI.UI;

namespace CompanySimulator.UI.Pages;

public class MainMenuPage : IPage, IStateListener
{
    private static readonly List<(ActionWithoutPayload, string)> MenuOptions = new()
    {
        (new DisplayPageAction(CompanySimulator.State.Pages.BuyTruck), "Buy truck"),
        (new DisplayPageAction(CompanySimulator.State.Pages.HireDriver), "Hire Driver"),
        (new DisplayPageAction(CompanySimulator.State.Pages.AcceptTender), "Accept Tender"),
        (new DisplayPageAction(CompanySimulator.State.Pages.AssignDriverToTruck), "Assign Driver to Truck"),
        (new DisplayPageAction(CompanySimulator.State.Pages.AssignTenderToTuck), "Assign Tender to Truck"),
        (new DisplayPageAction(CompanySimulator.State.Pages.RelocateTruck), "Relocate truck"),
        (new EndRoundAction(), "End Round"),
    };

    private readonly TextPrompt<int> _choicePrompt = new TextPrompt<int>($"Select option [1-{MenuOptions.Count}]")
        {
            Choices = Enumerable.Range(1, MenuOptions.Count).ToList(),
        }
        .ShowChoices(false);

    private readonly StateManager _stateManager;


    public MainMenuPage(StateManager stateManager)
    {
        _stateManager = stateManager;
        _stateManager.SubscribeToStateChanges(this);
    }

    private CompanyState State => _stateManager.CurrentState.CompanyState;
    private DateTime SimulationDate => _stateManager.CurrentState.SimulationState.SimulationDate;
    private string CompanyName => State.Name;
    private AccountBalance Balance => State.AccountBalance;
    private int TruckCount => State.OwnedTruckCount;
    private int DriverCount => State.EmployeeCount;
    private int TenderCount => State.AcceptedTenderCount;

    public void Render()
    {
        PrintInfoTable();
        PrintMenuOptions();
        var selectedOptionNum = AskUserToSelectMenuOption();
        HandleUserSelection(selectedOptionNum);
    }

    public void Dispose()
    {
        _stateManager.RemoveListener(this);
    }

    public void OnStateChanged(RootState oldState, RootState newState)
    {
        var dateHasChanged         = HasSimulationDateChanged(oldState, newState);
        var nameHasChanged         = HasNameChanged(oldState, newState);
        var balanceHasChanged      = HasBalanceChanged(oldState, newState);
        var ownedTruckCountChanged = HasOwnedTruckCountChanged(oldState, newState);
        var employeeCountChanged   = HasEmployeeCountChanged(oldState, newState);
        var tenderCountChanged     = HasTenderCountChanged(oldState, newState);

        if (!dateHasChanged && !nameHasChanged && !balanceHasChanged && !ownedTruckCountChanged && !employeeCountChanged && !tenderCountChanged) return;

        ConsoleUI.Console.Clear();
        Render();
    }

    private void HandleUserSelection(int selectedOptionNum)
    {
        var optionIdx = selectedOptionNum - 1;
        var (action, _) = MenuOptions.ElementAtOrDefault(optionIdx);
        if (action == null)
            return;

        _stateManager.DispatchAction(action);
    }

    private int AskUserToSelectMenuOption()
    {
        return ConsoleUI.Prompt(_choicePrompt);
    }

    private void PrintInfoTable()
    {
        ConsoleUI.Write(GetInfoTable());
    }

    private static void PrintMenuOptions()
    {
        var index = 1;
        foreach (var (_, text) in MenuOptions)
        {
            ConsoleUI.WriteLine($"{index}. {text}");
            index++;
        }

        ConsoleUI.WriteLine();
    }


    private Table GetInfoTable()
    {
        var table = new Table
        {
            Expand = true,
            Border = new VerticalTableBorders(),
        };
        const Justify center = Justify.Center;

        table.AddColumn(CompanyName, center);
        table.AddColumn(Balance.FormattedValueWithIsoCode, center);
        table.AddColumn(SimulationDate.Date.ToString("dd.MM.yyyy"), center);
        table.AddColumn($"{TruckCount} Trucks", center);
        table.AddColumn($"{DriverCount} Drivers", center);
        table.AddColumn($"{TenderCount} Accepted Tenders", center);

        return table;
    }

    private static bool HasSimulationDateChanged(RootState oldState, RootState newState)
    {
        var (oldDate, newDate) = GetNested(oldState, newState, s => s.SimulationState.SimulationDate);
        return oldDate.Date != newDate.Date;
    }

    private static bool HasNameChanged(RootState oldState, RootState newState)
    {
        var (oldName, newName) = GetNested(oldState, newState, s => s.CompanyState.Name);
        return oldName != newName;
    }

    private static bool HasBalanceChanged(RootState oldState, RootState newState)
    {
        var (oldBalance, newBalance) = GetNested(oldState, newState, s => s.CompanyState.AccountBalance);
        return oldBalance != newBalance;
    }

    private static bool HasOwnedTruckCountChanged(RootState oldState, RootState newState)
    {
        var (oldCount, newCount) = GetNested(oldState, newState, s => s.CompanyState.OwnedTruckCount);
        return oldCount != newCount;
    }

    private static bool HasEmployeeCountChanged(RootState oldState, RootState newState)
    {
        var (oldCount, newCount) = GetNested(oldState, newState, s => s.CompanyState.EmployeeCount);
        return oldCount != newCount;
    }

    private static bool HasTenderCountChanged(RootState oldState, RootState newState)
    {
        var (oldCount, newCount) = GetNested(oldState, newState, s => s.CompanyState.AcceptedTenderCount);
        return oldCount != newCount;
    }

    private static (TTarget, TTarget) GetNested<TTarget>(RootState oldState, RootState newState, Func<RootState, TTarget> selector)
    {
        var oldTarget = selector(oldState);
        var newTarget = selector(newState);

        return (oldTarget, newTarget);
    }
}