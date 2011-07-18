using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Tracking
{
    public interface ITrackProvider : IDisposable
    {
        Guid Id { get; }
        string Name { get; }
        string Category { get; }

        object this[string name] { get; set; }

        string[] PropertyNames {get;}

        TrackFactory Connection { get; }
    }
}
