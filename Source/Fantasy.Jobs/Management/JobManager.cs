using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Fantasy.ServiceModel;
using Fantasy.IO;
using Fantasy.XSerialization;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Management;
using System.Collections;
using System.IO;
using Fantasy.Jobs.Utils;
using System.Security.Cryptography;
using System.Threading;

namespace Fantasy.Jobs.Management
{
    public class JobManager : MarshalByRefObject, IJobManager
    {
        private JobManager()
        {

        }


        public override object InitializeLifetimeService()
        {
            return null;
        }

        private static JobManager _default = new JobManager();
        public static JobManager Default
        {
            get
            {
                return _default;
            }
        }

        private ServiceContainer _serviceContainer = new ServiceContainer();

        private string GetContentString(XElement element)
        {
            StringBuilder rs = new StringBuilder();
            foreach (XElement child in element.Descendants())
            {
                foreach (XAttribute attr in child.Attributes())
                {
                    if (!attr.IsNamespaceDeclaration)
                    {
                        rs.Append(attr.Name.ToString() + attr.Value.Trim());
                    }
                }
                if (!child.HasElements)
                {
                    rs.Append(child.Name.ToString() + child.Value.Trim());
                }
            }
            return rs.ToString();
        }



#if !DEBUG
        private bool CheckLicence()
        {
            bool rs = false;
            string c = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            string p = LongPath.ChangeExtension(c, "lic");
            if (LongPathFile.Exists(p))
            {
                XElement root = LongPathXNode.LoadXElement(p);
                XNamespace ns = Consts.LicenceNamespaceURI;
                string signature = (string)root.Attribute("signature");
                string content = GetContentString(root);
                byte[] tb;
                bool isValidSignature = false;
                using (SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider())
                {
                    tb = sha.ComputeHash(Encoding.Unicode.GetBytes(content));
                }
                using (DSACryptoServiceProvider dsa = new DSACryptoServiceProvider())
                {
                    dsa.FromXmlString(Fantasy.Jobs.Properties.Resources.FantasyPubKey);
                    DSASignatureDeformatter formatter = new DSASignatureDeformatter(dsa);
                    formatter.SetHashAlgorithm("SHA1");
                    isValidSignature = formatter.VerifySignature(tb, Convert.FromBase64String(signature));
                }

                if (isValidSignature)
                {
                    XSerializer ser = new XSerializer(typeof(Licence));
                    XElement le = root.Element(ns + "jobservice");
                    if (le != null)
                    {
                        Licence l = (Licence)ser.Deserialize(le);

                        if ((GetMachineCode() == l.MachineCode || l.MachineCode == String.Empty) && DateTime.Now < l.ExpireTime)
                        {
                            this._serviceContainer.AddService(l);
                            ThreadFactory.CreateThread(state =>
                            {
                                DateTime t = (DateTime)state;
                                while (DateTime.Now < t)
                                {
                                    Thread.Sleep(TimeSpan.FromMinutes(5));
                                }

                                ILogger logger = this.GetService<ILogger>();
                                logger.LogError("Licence", "Licence is invalid or expired.");
                                this.Stop();
                            }).WithStart(l.ExpireTime);

                            rs = true;
                            if (l.IsDevelopVersion && Environment.ProcessorCount > 1)
                            {

                                Random r = new Random();
                                int i = r.Next(Environment.ProcessorCount);

                                uint m = 1u << i;

                                SetProcessAffinityMask(Process.GetCurrentProcess().Handle, m);

                            }
                        }

                    }

                }
            }

            return rs;
        }
#else
        private bool CheckLicence()
        {
            return true;
        }
#endif

        private struct HardwareKey
        {
            public string Key;

            public string[] Properties;
        }

        private string GetMachineCode()
        {
            string rs;
            HardwareKey[] keys = new HardwareKey[]
            {
                new HardwareKey {Key = "Win32_Processor", Properties= new string[] {"ProcessorId", "Name", "Manufacturer", "MaxClockSpeed"}},
                new HardwareKey {Key = "Win32_BIOS", Properties=new string[] {"Manufacturer", "SMBIOSBIOSVersion", "IdentificationCode", "SerialNumber", "Version" }}
            };

            MemoryStream ms = new MemoryStream();
            StreamWriter w = new StreamWriter(ms);


            foreach (HardwareKey key in keys)
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + key.Key);
                foreach (ManagementObject mo in searcher.Get())
                {
                    foreach (string p in key.Properties)
                    {
                        object o = mo[p];
                        if (o != null)
                        {
                            if (o.GetType().IsArray)
                            {
                                w.Write(string.Join("", (IEnumerable)o));

                            }
                            else
                            {
                                w.Write(o.ToString());
                            }
                        }
                    }
                }

            }

            ms.Seek(0, SeekOrigin.Begin);

            using (SHA1CryptoServiceProvider provider = new SHA1CryptoServiceProvider())
            {

                byte[] bytes = provider.ComputeHash(ms);
                rs = BitConverter.ToString(bytes).Replace("-", "");

            }

            return rs;

        }

        [DllImport("kernel32.dll", EntryPoint = "SetProcessAffinityMask")]
        private static extern bool SetProcessAffinityMask(IntPtr hProcess,
           uint dwProcessAffinityMask);

        public void Start(IServiceProvider parentServices = null)
        {
            List<object> services = new List<object>(AddIn.CreateObjects("jobService/services/service"));
            services.Add(this);
            _serviceContainer.InitializeServices(parentServices, services.ToArray());
            if (CheckLicence())
            {
                object[] commands = AddIn.CreateObjects("jobService/startupCommands/command");

                foreach (ICommand command in commands)
                {
                    if (command is IObjectWithSite)
                    {
                        ((IObjectWithSite)command).Site = this;
                    }
                    command.Execute(null);
                }
            }
            else
            {
                ILogger logger = this.GetService<ILogger>();
                logger.LogError("Licence", "Licence is invalid or expired.");

                Stop();
            }
        }

        private bool _stopped = false;

        public void Stop()
        {
            if (!_stopped)
            {
                this._stopped = true;
                this._serviceContainer.UninitializeServices();
            }
        }

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            return _serviceContainer.GetService(serviceType);
        }

        #endregion
    }
}
