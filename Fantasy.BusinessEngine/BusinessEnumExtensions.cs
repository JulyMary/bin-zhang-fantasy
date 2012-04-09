using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Properties;

namespace Fantasy.BusinessEngine
{
    public static class BusinessEnumExtensions
    {
        public static Type RuntimeType(this BusinessEnum @enum)
        {
            string typeName;
            if (@enum.IsExternal)
            {
                typeName = String.Format("{0}.{1}, {2}", @enum.ExternalNamespace, @enum.CodeName, @enum.ExternalAssemblyName);
            }
            else
            {
                typeName = String.Format("{0}, {1}", @enum.FullCodeName, Settings.Default.BusinessDataAssemblyName);
            }

            return Type.GetType(typeName, true);
        }
    }
}
