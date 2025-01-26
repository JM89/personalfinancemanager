using System.ComponentModel.DataAnnotations;

namespace PFM.Models;

public class BankListModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string CountryName { get; set; } = string.Empty;

    public string IconPath { get; set; } = string.Empty;

    public string? RenderedIcon { get; set; }

    public bool CanBeDeleted { get; set; }
}
	
public class BankEditModel
{
    public int? Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int CountryId { get; set; }

    public CountryModel Country { get; set; } = new ();

    [MaxLength(200)]
    [Required]
    public string Website { get; set; } = string.Empty;

    [RegularExpression(@"[0-9]{11}", ErrorMessage = "Not a valid Phone number (00000000000)")]
    [Required]
    public string GeneralEnquiryPhoneNumber { get; set; } = string.Empty;

    public string IconPath { get; set; } = string.Empty;

    public string? RenderedIcon { get; set; }

    public DisplayIcon DisplayIconFlags { get; set; }
}

[Flags]
public enum DisplayIcon
{
    None = 0,
    DisplayUploader = 1,
    DisplayExistingIcon = 2,
    DisplayIconPathPreview = 4,
    DisplayFilePreview = 8
}