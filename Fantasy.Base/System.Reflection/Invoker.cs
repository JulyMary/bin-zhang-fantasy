

namespace System.Reflection
{
    public static class Invoker
    {
        public static object Invoke(object instance, string member)
        {
            object rs = instance;
            if (!string.IsNullOrEmpty(member) && rs != null)
            {
                foreach(string name in member.Split(new char[] {'.'}, StringSplitOptions.RemoveEmptyEntries))
                {
                    rs = InvokeSegment(rs, name);
                    if (rs == null)
                    {
                        break;
                    }
                }
            }

            return rs;
        }

        public static T Invoke<T>(object instance, string member)
        {
            return (T)Invoke(instance, member); 
        }

        private static object InvokeSegment(object instance, string name)
        {
            Type t = instance.GetType();

            object rs = t.InvokeMember(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Instance, null, instance, null);
            return rs;

        }

        public static object InvokeStatic(Type type, string member)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (string.IsNullOrWhiteSpace(member))
            {
                throw new ArgumentException(string.Format(Fantasy.Properties.Resources.ArgumentNullOrEmptyStringText, "member"), "member");
            }

            string[] names = member.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            PropertyInfo pi = type.GetProperty(names[0], System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty);
            object rs = pi.GetValue(null, null);

            for (int i = 1; i < names.Length && rs != null; i++)
            {
                rs = InvokeSegment(rs, names[i]);
            }

            return rs;
        }



    }
}
