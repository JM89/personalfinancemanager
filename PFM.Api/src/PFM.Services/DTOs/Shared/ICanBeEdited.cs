namespace PFM.Services.DTOs.Shared
{
    public interface ICanBeEdited
    {
        bool CanBeEdited { get; set; }

        string CanBeEditedTooltipResourceName { get; }
    }
}
