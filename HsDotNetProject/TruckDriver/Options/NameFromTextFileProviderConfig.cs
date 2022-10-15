using System.ComponentModel.DataAnnotations;

namespace TruckDriver.Options;

public class NameFromTextFileProviderConfig
{
    [Required] public string FilePath { get; set; } = null!;
}