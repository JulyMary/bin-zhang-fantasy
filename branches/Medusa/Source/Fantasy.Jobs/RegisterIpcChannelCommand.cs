using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels.Ipc;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using System.Collections;
using System.Runtime.Serialization.Formatters;

namespace Fantasy.Jobs
{
    public class RegisterIpcChannelCommand : ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            IDictionary props = new Hashtable();
            props["secure"] = true;
            props["tokenImpersonationLevel"] = "impersonation";
            props["authorizedGroup"] = "Everyone";
            props["portName"] = Process.GetCurrentProcess().ProcessName + ":" + Process.GetCurrentProcess().Id.ToString();
            IpcChannel ipc = new IpcChannel(props, new BinaryClientFormatterSinkProvider(), new BinaryServerFormatterSinkProvider() { TypeFilterLevel = TypeFilterLevel.Full });
            ChannelServices.RegisterChannel(ipc, true);
            return null;

        }

        #endregion
    }
}
