﻿namespace Api.Contracts.Shared
{
    public interface ICanBeEdited
    {
        bool CanBeEdited { get; set; }

        string CanBeEditedTooltipResourceName { get; }
    }
}
