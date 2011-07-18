using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Tracking
{
    public class TrackChangedEventArgs : EventArgs
    {
        public TrackChangedEventArgs(string name, object oldValue, object newValue)
        {
            this.Name = name;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public string Name { get; private set; }
        public object OldValue { get; private set; }
        public object NewValue { get; private set; }

    }

    public class TrackActivedEventArgs : EventArgs
    {
        public TrackActivedEventArgs(TrackMetaData data)
        {
            this.MetaData = data;
        }

        public TrackMetaData MetaData { get; private set; }
    }  
}
