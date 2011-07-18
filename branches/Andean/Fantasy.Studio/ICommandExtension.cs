using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Fantasy.Studio
{
    public static class ICommandExtension
    {

        private static Dictionary<ICommand, ICommandData> _storage = new Dictionary<ICommand, ICommandData>();
        private static object _syncRoot = new object();
        public static ICommandData CommandData(this ICommand cmd)
        {
            lock(_syncRoot)
            {
                ICommandData rs;
                if (!_storage.TryGetValue(cmd, out rs))
                {
                    rs = new ICommandData();
                    _storage.Add(cmd, rs);
                }
                return rs;
            }
        }
    }
}
