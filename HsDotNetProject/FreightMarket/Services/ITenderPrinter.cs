using FreightMarket.Models;

namespace FreightMarket.Services;

public interface ITenderPrinter
{
    void PrintTenders(IList<TransportationTender> tenders);
}