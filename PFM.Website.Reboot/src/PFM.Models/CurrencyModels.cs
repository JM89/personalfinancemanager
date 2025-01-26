using System.ComponentModel.DataAnnotations;

namespace PFM.Models;

public class CurrencyModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(3)]
    public string Symbol { get; set; } = string.Empty;
}