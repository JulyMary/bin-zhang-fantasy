using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Tracking
{
    public static class TrackExtension
    {
        public static T GetProperty<T>(this ITrackProvider provider, string name, T defaultValue)
        {
            object obj = provider[name];
            if (obj != null)
            {
                return (T)obj;
               
            }

            else
            {

                return defaultValue;

            }
        }


        public static T GetProperty<T>(this ITrackListener listener, string name, T defaultValue)
        {
            object obj = listener[name];
            if (obj != null)
            {
                return (T)obj;

            }

            else
            {

                return defaultValue;

            }
        }
    }
}
