using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Fantasy.ServiceModel
{
   
    [DataContract]
    public class CallbackExpiredException
    {
        public CallbackExpiredException() 
        {
            this.Message = "Callback expired.";
        }

        [DataMember]
        public string Message { get; set; }
    }
}
