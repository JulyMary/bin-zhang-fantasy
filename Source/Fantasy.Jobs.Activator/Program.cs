using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Management;
using System.Collections;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Fantasy.Jobs.Activator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Activator Key:");
            string key = GetMachineCode();
            Console.WriteLine(key);
          
        }

        private struct HardwareKey
        {
            public string Key;

            public string[] Properties;
        }

        private static string GetMachineCode()
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
    }
}
