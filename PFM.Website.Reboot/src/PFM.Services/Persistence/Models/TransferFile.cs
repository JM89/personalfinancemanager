namespace PFM.Services.Persistence.Models;

public class TransferFile
{
    public string Name { get; set; } = string.Empty;
    public MemoryStream? Stream { get; set; }
}