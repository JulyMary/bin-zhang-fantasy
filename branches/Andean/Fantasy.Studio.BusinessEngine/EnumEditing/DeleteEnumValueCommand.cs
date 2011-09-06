using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class DeleteEnumValueCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {

            BusinessEnumValue[] values = ((IEnumerable<BusinessEnumValue>)args).ToArray();
            foreach (BusinessEnumValue value in values)
            {
                value.Enum.EnumValues.Remove(value);
            }

            return null;

        }

        #endregion
    }
}
