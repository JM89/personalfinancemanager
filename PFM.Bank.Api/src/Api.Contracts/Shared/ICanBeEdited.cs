// ReSharper disable once CheckNamespace
namespace PFM.Bank.Api.Contracts.Shared
{
    public interface ICanBeEdited
    {
        bool CanBeEdited { get; set; }

        string CanBeEditedTooltipResourceName { get; }
    }
}
