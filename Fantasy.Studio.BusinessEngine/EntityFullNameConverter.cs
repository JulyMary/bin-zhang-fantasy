using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine
{
    public class EntityFullNameConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            INamedBusinessEntity entity = value as INamedBusinessEntity;
            return entity != null ? entity.FullName : null;
        }
    }
}
