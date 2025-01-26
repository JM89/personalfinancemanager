using System.ComponentModel.DataAnnotations;

namespace PFM.Models;

public class ExpenseTypeModel
{
    public int? Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string GraphColor { get; set; } = string.Empty;

    [Required]
    public bool ShowOnDashboard { get; set; }
}