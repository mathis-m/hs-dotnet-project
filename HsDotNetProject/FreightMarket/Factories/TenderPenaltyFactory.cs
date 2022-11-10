using FreightMarket.Models;

namespace FreightMarket.Factories;

public static class TenderPenaltyFactory
{
    private static readonly Random Random = new();

    public static TenderPenalty FromCompensation(TenderCompensation compensation)
    {
        var multiplier = Random.Next(50, 200);
        var penalty    = multiplier * compensation.Value / 100;
        return new TenderPenalty(penalty, compensation.Currency);
    }
}