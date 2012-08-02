package fantasy.jobs.management;

import java.rmi.*;
import java.rmi.server.*;
import java.util.Arrays;

import fantasy.*;
import fantasy.servicemodel.*;

public class JobManager extends UnicastRemoteObject implements IJobManager
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 4653233011893159359L;
	private JobManager() throws RemoteException
	{

	}

    
	static
	{
		try {
			_default = new JobManager();
		} catch (RemoteException e) {
			
			e.printStackTrace();
		}
	}
	

	private static JobManager _default;
	public static JobManager getDefault()
	{
		return _default;
	}

	private ServiceContainer _serviceContainer = new ServiceContainer();

/*	private String GetContentString(Element element)
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

			SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
			try
			{
				tb = sha.ComputeHash(Encoding.Unicode.GetBytes(content));
			}
			finally
			{
				sha.dispose();
			}
			DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
			try
			{
				dsa.FromXmlString(fantasy.jobs.properties.Resources.getFantasyPubKey());
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

	*/
	private boolean CheckLicence()
	{
		return true;
	}


	public final void Start(IServiceProvider parentServices) throws Exception
	{
		
		java.util.ArrayList<Object> services = new java.util.ArrayList<Object>(Arrays.asList(AddIn.CreateObjects("jobService/services/service")));
		services.add(this);
		_serviceContainer.initializeServices(parentServices, services.toArray(new Object[]{}));
		if (CheckLicence())
		{
			ICommand[] commands = AddIn.CreateObjects(ICommand.class, "jobService/startupCommands/command");

			for (ICommand command : commands)
			{
				if (command instanceof IObjectWithSite)
				{
					((IObjectWithSite)command).setSite(this);
				}
				command.Execute(null);
			}
		}
		else
		{
			ILogger logger = this.getService(ILogger.class);
			Log.SafeLogError(logger, "Licence", "Licence is invalid or expired.");
			

			Stop();
		}
	}

	private boolean _stopped = false;

	public final void Stop() throws Exception
	{
		try
		{
			if (!_stopped)
			{
				this._stopped = true;
				this._serviceContainer.uninitializeServices();
			}
		}
		finally
		{
			 UnicastRemoteObject.unexportObject(this, true); 
		}
	}


	public final <T> T getService(java.lang.Class<T> serviceType) throws Exception
	{
		return _serviceContainer.getService(serviceType);
	}
	
	@Override
	public Object getService2(@SuppressWarnings("rawtypes") Class serviceType) throws Exception {
		return this._serviceContainer.getService2(serviceType);
	}



	@Override
	public Object getRequiredService2(@SuppressWarnings("rawtypes") Class serviceType) throws Exception {
		return this._serviceContainer.getRequiredService2(serviceType);
	}



	@Override
	public <T> T getRequiredService(Class<T> serviceType) throws Exception {
		return this._serviceContainer.getRequiredService(serviceType);
	}


}