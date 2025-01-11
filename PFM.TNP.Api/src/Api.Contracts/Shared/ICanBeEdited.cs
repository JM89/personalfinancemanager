// ReSharper disable once CheckNamespace
namespace PFM.TNP.Api.Contracts.Shared
{
    public interface ICanBeEdited
    {
        bool CanBeEdited { get; set; }

        string CanBeEditedTooltipResourceName { get; }
    }
}
