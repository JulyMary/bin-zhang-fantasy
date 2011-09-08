using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Adaption;
using Fantasy.BusinessEngine;
using System.ComponentModel;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    public class EnumGlyphAdapterFactory : ObjectWithSite, IAdapterFactory
    {
        #region IAdapterFactory Members

        public object GetAdapter(object adaptee, Type targetType)
        {
            if (adaptee is EnumGlyph)
            {
                EnumGlyph node = (EnumGlyph)adaptee;

                if (targetType == typeof(BusinessEnum))
                {
                    return node.Entity;
                }
                else if (targetType == typeof(ICustomTypeDescriptor))
                {
                    return this.Site.GetRequiredService<IAdapterManager>().GetAdapter<ICustomTypeDescriptor>(node.Entity);
                }
            }

            return null;
        }

        public Type[] GetTargetTypes()
        {
            return new Type[] { typeof(BusinessEnum), typeof(ICustomTypeDescriptor) };
        }

        #endregion
    }
}
