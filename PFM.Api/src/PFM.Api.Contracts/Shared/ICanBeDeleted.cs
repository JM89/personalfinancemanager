namespace PFM.Api.Contracts.Shared
{
    public interface ICanBeDeleted
    {
        bool CanBeDeleted { get; set; }

        string TooltipResourceName { get; }
    }
}
