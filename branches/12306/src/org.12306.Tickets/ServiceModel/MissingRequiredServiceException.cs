using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org._12306.Tickets.Properties;

namespace Org._12306.Tickets.ServiceModel
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
