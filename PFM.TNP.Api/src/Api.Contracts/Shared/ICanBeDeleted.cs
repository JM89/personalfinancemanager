// ReSharper disable once CheckNamespace
namespace PFM.TNP.Api.Contracts.Shared
{
    public interface ICanBeDeleted
    {
        bool CanBeDeleted { get; set; }

        string TooltipResourceName { get; }
    }
}
