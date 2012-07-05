using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Tracking
{
    public interface ITrackListener : IDisposable
    {
        Guid Id { get; }
        string Name { get; }
        string Category { get; }

        object this[string name] { get; }

        string[] PropertyNames { get; }

        TrackFactory Connection { get; }

        bool IsActived { get;}

        event EventHandler ActiveStateChanged;

        event EventHandler<TrackChangedEventArgs> Changed;
    }
}
