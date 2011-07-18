using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Properties;

namespace Fantasy.ServiceModel
{
    [Serializable]
    public class MissingRequiredServiceException : Exception 
    {
        public MissingRequiredServiceException(Type serviceType)
            : base(string.Format(Resources.MissingRequiredServiceText, serviceType))
        {

        }
    }
}
