using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.IO;

namespace Fantasy.IO
{
    public class WNet
    {
        // Methods
        public static bool TryAddConnection(string resourceName, string userName, string password, out string message)
        {
            resourceName = resourceName.TrimEnd('/', '\\');
            message = null;
            NETRESOURCE netResource = new NETRESOURCE()
            {
                RemoteName = resourceName  
            };
            
            int hr = WNetAddConnection3(IntPtr.Zero, ref netResource, ref password, ref userName, 0);
            if (hr != 0)
            {
                Win32Exception ex = new Win32Exception(hr);
                message = ex.Message;
            }
            return (hr == 0);
        }



        public static void AddConnection(string resourceName, string userName, string password)
        {
            resourceName = resourceName.TrimEnd('/', '\\');
            NETRESOURCE netResource = new NETRESOURCE()
            {
                RemoteName = resourceName
            };

            int hr = WNetAddConnection3(IntPtr.Zero, ref netResource, ref password, ref userName, 0);
            if (hr != 0)
            {
               throw new Win32Exception(hr);
            }
          
        }

        [DllImport("mpr.dll", EntryPoint = "WNetAddConnection3A", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int WNetAddConnection3(IntPtr handler, ref NETRESOURCE netResource, [MarshalAs(UnmanagedType.VBByRefStr)] ref string password, [MarshalAs(UnmanagedType.VBByRefStr)] ref string userName, int flags);
        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        private struct NETRESOURCE
        {
            public int dwScope;
            public int dwType;
            public int dwDisplayType;
            public int dwUsage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }
    }


}
