using System.Collections;
using System.Collections.Generic;

namespace PFM.Services.Events.Interfaces
{
    public interface IEvent
    {
        string Id { get; }

        string StreamGroup { get; }
    }
}
