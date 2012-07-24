package fantasy.jobs.management;

import Fantasy.ServiceModel.*;
import Fantasy.IO.*;
import Fantasy.XSerialization.*;
import Fantasy.Jobs.Utils.*;

public class JobManager extends MarshalByRefObject implements IJobManager
{
	private JobManager()
	{

	}


	@Override
	public Object InitializeLifetimeService()
	{
		return null;
	}

	private static JobManager _default = new JobManager();
	public static JobManager getDefault()
	{
		return _default;
	}

	private ServiceContainer _serviceContainer = new ServiceContainer();

	private String GetContentString(XElement element)
	{
		StringBuilder rs = new StringBuilder();
		for (XElement child : element.Descendants())
		{
			for (XAttribute attr : child.Attributes())
			{
				if (!attr.IsNamespaceDeclaration)
				{
					rs.append(attr.getName().toString() + attr.getValue().trim());
				}
			}
			if (!child.HasElements)
			{
				rs.append(child.getName().toString() + child.getValue().trim());
			}
		}
		return rs.toString();
	}



//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
//#if !DEBUG
	private boolean CheckLicence()
	{
		boolean rs = false;
		String c = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
		String p = LongPath.ChangeExtension(c, "lic");
		if (LongPathFile.Exists(p))
		{
			XElement root = LongPathXNode.LoadXElement(p);
			XNamespace ns = Consts.LicenceNamespaceURI;
			String signature = (String)root.Attribute("signature");
			String content = GetContentString(root);
			byte[] tb;
			boolean isValidSignature = false;
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//			using (SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider())
			SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
			try
			{
				tb = sha.ComputeHash(Encoding.Unicode.GetBytes(content));
			}
			finally
			{
				sha.dispose();
			}
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//			using (DSACryptoServiceProvider dsa = new DSACryptoServiceProvider())
			DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
			try
			{
				dsa.FromXmlString(Fantasy.Jobs.Properties.Resources.getFantasyPubKey());
				DSASignatureDeformatter formatter = new DSASignatureDeformatter(dsa);
				formatter.SetHashAlgorithm("SHA1");
				isValidSignature = formatter.VerifySignature(tb, Convert.FromBase64String(signature));
			}
			finally
			{
				dsa.dispose();
			}

			if (isValidSignature)
			{
				XSerializer ser = new XSerializer(Licence.class);
				XElement le = root.Element(ns + "jobservice");
				if (le != null)
				{
					Licence l = (Licence)ser.Deserialize(le);

					if (GetMachineCode().equals(l.MachineCode) && new java.util.Date() < l.ExpireTime)
					{
						this._serviceContainer.AddService(l);
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
						ThreadFactory.CreateThread(state =>
						{
							java.util.Date t = (java.util.Date)state;
							while (new java.util.Date() < t)
							{
								Thread.sleep(TimeSpan.FromMinutes(5));
							}
							ILogger logger = this.<ILogger>GetService();
							logger.LogError("Licence", "Licence is invalid or expired.");
							this.Stop();
						}
					   ).WithStart(l.ExpireTime);

						rs = true;
						if (l.IsDevelopVersion && Environment.ProcessorCount > 1)
						{

							java.util.Random r = new java.util.Random();
							int i = r.nextInt(Environment.ProcessorCount);

//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: uint m = 1u << i;
							int m = 1u << i;

							SetProcessAffinityMask(Process.GetCurrentProcess().Handle, m);

						}
					}

				}

			}
		}

		return rs;
	}
//#else
	private boolean CheckLicence()
	{
		return true;
	}
//#endif

//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: private struct HardwareKey
	private final static class HardwareKey
	{
		public String Key;

		public String[] Properties;

		public HardwareKey clone()
		{
			HardwareKey varCopy = new HardwareKey();

			varCopy.Key = this.Key;
			varCopy.Properties = this.Properties;

			return varCopy;
		}
	}

	private String GetMachineCode()
	{
		String rs;
		HardwareKey tempVar = new HardwareKey();
		tempVar.Key = "Win32_Processor";
		tempVar.Properties= new String[] {"ProcessorId", "Name", "Manufacturer", "MaxClockSpeed"};
		HardwareKey tempVar2 = new HardwareKey();
		tempVar2.Key = "Win32_BIOS";
		tempVar2.Properties = new String[] {"Manufacturer", "SMBIOSBIOSVersion", "IdentificationCode", "SerialNumber", "Version" };
		HardwareKey[] keys = new HardwareKey[] { tempVar, tempVar2 };

		MemoryStream ms = new MemoryStream();
		StreamWriter w = new StreamWriter(ms);


		for (HardwareKey key : keys)
		{
			ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + key.getKey());
			for (ManagementObject mo : searcher.Get())
			{
				for (String p : key.Properties)
				{
					Object o = mo[p];
					if (o != null)
					{
						if (o.getClass().IsArray)
						{
							w.Write(DotNetToJavaStringHelper.join("", (Iterable)o));

						}
						else
						{
							w.Write(o.toString());
						}
					}
				}
			}

		}

		ms.Seek(0, SeekOrigin.Begin);

//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (SHA1CryptoServiceProvider provider = new SHA1CryptoServiceProvider())
		SHA1CryptoServiceProvider provider = new SHA1CryptoServiceProvider();
		try
		{

			byte[] bytes = provider.ComputeHash(ms);
			rs = BitConverter.ToString(bytes).replace("-", "");

		}
		finally
		{
			provider.dispose();
		}

		return rs;

	}

//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: private static extern bool SetProcessAffinityMask(IntPtr hProcess, uint dwProcessAffinityMask);
	private static native boolean SetProcessAffinityMask(IntPtr hProcess, int dwProcessAffinityMask);
	static
	{
		System.loadLibrary("kernel32.dll");
	}

//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: public void Start(IServiceProvider parentServices = null)
	public final void Start(IServiceProvider parentServices)
	{
		java.util.ArrayList<Object> services = new java.util.ArrayList<Object>(AddIn.CreateObjects("jobService/services/service"));
		services.add(this);
		_serviceContainer.InitializeServices(parentServices, services.toArray(new Object[]{}));
		if (CheckLicence())
		{
			Object[] commands = AddIn.CreateObjects("jobService/startupCommands/command");

			for (ICommand command : commands)
			{
				if (command instanceof IObjectWithSite)
				{
					((IObjectWithSite)command).Site = this;
				}
				command.Execute(null);
			}
		}
		else
		{
			ILogger logger = this.<ILogger>GetService();
			logger.LogError("Licence", "Licence is invalid or expired.");

			Stop();
		}
	}

	private boolean _stopped = false;

	public final void Stop()
	{
		if (!_stopped)
		{
			this._stopped = true;
			this._serviceContainer.UninitializeServices();
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IServiceProvider Members

	public final Object GetService(java.lang.Class serviceType)
	{
		return _serviceContainer.GetService(serviceType);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}