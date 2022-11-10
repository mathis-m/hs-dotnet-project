using Common.Services;
using VehicleAcquisition.Models;

namespace VehicleAcquisition.Services;

public class TruckAgeRandomizerService : BaseRandomizerService<Age>
{
    private static readonly Random AgeRndm = new();

    public override Task<Age> NextAsync()
    {
        return Task.FromResult(new Age(AgeRndm.Next(0, 11)));
    }
}