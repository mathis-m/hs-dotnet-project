using System.ComponentModel.DataAnnotations;

namespace TruckDriver.Options;

public class DriverCategoryConfig
{
    [Required]
    public List<string> AvailableTypes { get; set; } = null!;
}