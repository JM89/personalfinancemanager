// ReSharper disable once CheckNamespace
namespace PFM.Bank.Api.Contracts.Shared
{
    public interface ICanBeDeleted
    {
        bool CanBeDeleted { get; set; }

        string TooltipResourceName { get; }
    }
}
