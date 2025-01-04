using Api.Contracts.Shared;

// ReSharper disable once CheckNamespace
// Contracts are reused in different project and should not clash
namespace PFM.Pension.Api.Contracts.Pension;

public class PensionList : ICanBeDeleted
{
    public bool CanBeDeleted { get; set; }

    public string TooltipResourceName => "BankCantBeDeleted";
}