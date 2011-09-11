using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fantasy.Windows
{
    public static class DataObjectExtension
    {

        public static T GetDataByType<T>(this IDataObject dataObject)
        {
            return (T)dataObject.GetDataByType(typeof(T));
        }

        public static object GetDataByType(this IDataObject dataObject, Type type)
        {

            foreach (string format in dataObject.GetFormats())
            {
                object rs = dataObject.GetData(format);

                if (type.IsInstanceOfType(rs))
                {
                    return rs;
                }
            }

            return null;

        }
    }
}
