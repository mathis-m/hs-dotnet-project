using System.ComponentModel.DataAnnotations;

namespace TruckDriver.Options;

public class SalaryExpectationLimitsConfig
{
    [Required]
    public int LowerLimit { get; set; }

    [Required]
    public int UpperLimit { get; set; }

    [Required]
    public string CurrencyIso { get; set; } = null!;

    [Required]
    public string CurrencySymbol { get; set; } = null!;
}