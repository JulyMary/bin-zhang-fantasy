using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using NHibernate;

namespace Fantasy.Studio.BusinessEngine.PropertyEditing
{
    public class RemovePropertyCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {

            BusinessProperty[] properties = ((IEnumerable<BusinessProperty>)args).ToArray();
            foreach (BusinessProperty prop in properties)
            {
                prop.Class.Properties.Remove(prop);
            }

            return null;

        }

        #endregion
    }
}
