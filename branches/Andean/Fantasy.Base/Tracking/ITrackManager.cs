using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Tracking
{
    public interface ITrackManager
    {

        TrackFactory Connection { get; }

        TrackMetaData[] GetActivedTrackMetaData();

        event EventHandler<TrackActivedEventArgs> TrackActived; 

    }
}
