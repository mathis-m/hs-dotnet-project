using CompanySimulator.State;
using CompanySimulator.State.Actions;
using CompanySimulator.UI.Utils.TableUtils;
using FreightMarket.Models;
using UconsoleI.Components.TableComponent;
using UconsoleI.Rendering;

namespace CompanySimulator.UI.Pages;

public class AcceptTenderPage : BaseTablePage<TransportationTender>
{
    public AcceptTenderPage(StateManager stateManager) : base(stateManager)
    {
    }

    protected override string Prompt => "Accept a tender";

    protected override List<TableColumn> TableColumns => TenderTableUtil.TableColumns;

    protected override IReadOnlyList<TransportationTender> GetSimulationList(RootState state)
    {
        return state.SimulationState.OpenTenders;
    }

    protected override ActionWithPayload<TransportationTender> GetOnSelectedAction(TransportationTender tender)
    {
        return new AcceptTenderAction(tender);
    }

    protected override IEnumerable<IComponent> GetTableRow(int num, TransportationTender tender)
    {
        return TenderTableUtil.GetTableRow(num, tender);
    }
}