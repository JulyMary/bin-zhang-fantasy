using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Fantasy.Tracking
{
    // Use a data contract as illustrated in the sample below to add composite types to service operations
    [DataContract]
    public class TrackProperty
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string TypeName { get; set; }


        internal static TrackProperty Create(string name, object value)
        {
            TrackProperty rs = new TrackProperty() {Name = name};
            if(value != null)
            {
                rs.Value = value.ToString();
                rs.TypeName = value.GetType().FullName;
            }
            return rs;

        }

        internal static object ToObject(TrackProperty property)
        {
            if (property.TypeName != null)
            {
                Type t = Type.GetType(property.TypeName);
                return Convert.ChangeType(property.Value, t);
            }
            else
            {
                return null;
            }
        }
    }
}
