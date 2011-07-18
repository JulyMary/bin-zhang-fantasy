using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Adaption;
using Fantasy.ServiceModel;

namespace Fantasy.Studio.Codons
{
    public class InputCommandAdapterFactory : IAdapterFactory
    {

        #region IAdapterFactory Members

        public object GetAdapter(object adaptee, Type targetType)
        {
            return new InputCommandAdapter((ICommand)adaptee);
        }

        public Type[] GetTargetTypes()
        {
            return new Type[] { typeof(System.Windows.Input.ICommand) };
        }

        #endregion


    }
}
