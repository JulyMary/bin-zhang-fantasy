package fantasy.jobs.resources;

import Fantasy.IO.*;

public class UncPathResourceProvider extends ObjectWithSite implements IResourceProvider
{

	private static class PathInfo
	{
		private String privatePath;
		public final String getPath()
		{
			return privatePath;
		}
		public final void setPath(String value)
		{
			privatePath = value;
		}
		private String privateUser;
		public final String getUser()
		{
			return privateUser;
		}
		public final void setUser(String value)
		{
			privateUser = value;
		}
		private String privatePassword;
		public final String getPassword()
		{
			return privatePassword;
		}
		public final void setPassword(String value)
		{
			privatePassword = value;
		}
	}

	private Thread _refreshThread;

	private java.util.ArrayList<PathInfo> _requestedPaths = new java.util.ArrayList<PathInfo>();

	private Object _syncRoot = new Object();

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IResourceProvider Members

	public final boolean CanHandle(String name)
	{
		return String.equals(name, "unc", StringComparison.OrdinalIgnoreCase);
	}

	public final void Initialize()
	{
		_refreshThread = ThreadFactory.CreateThread(Refresh).WithStart();

	}

	private void Refresh()
	{
		while (true)
		{
			Thread.sleep(60 * 1000);
			java.util.ArrayList<String> connects = new java.util.ArrayList<String>();
			java.util.ArrayList<PathInfo> temp;
			synchronized (_syncRoot)
			{
				temp = new java.util.ArrayList<PathInfo>(this._requestedPaths);
			}

			for (PathInfo pi : temp)
			{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				if (!connects.Any(s => String.equals(pi.getPath(), pi.getPath(), StringComparison.OrdinalIgnoreCase)))
				{
					try
					{
						WNet.AddConnection(pi.getPath(), pi.getUser(), pi.getPassword());
						connects.add(pi.getPath());
					}
					catch (java.lang.Exception e)
					{
					}
				}
			}

			synchronized (_syncRoot)
			{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				var removing = from pi in temp where connects.Any(s => String.equals(pi.Path, s, StringComparison.OrdinalIgnoreCase)) select pi;
				for (PathInfo pi : removing)
				{
					_requestedPaths.remove(pi);
				}
			}

			if (connects.size() > 0)
			{
				this.OnAvailable(EventArgs.Empty);
			}


		}
	}

	public final boolean IsAvailable(ResourceParameter parameter)
	{
		String path = parameter.getValues().GetValueOrDefault("path");
		boolean isUnc = false;
		if (!String.IsNullOrWhiteSpace(path))
		{
			try
			{
				Uri uri = new Uri(path);
				isUnc = uri.IsUnc;
			}
			catch (java.lang.Exception e)
			{

			}

			if (isUnc)
			{
				boolean connected = false;
				try
				{
					connected = LongPathDirectory.Exists(path);
				}
				catch (java.lang.Exception e2)
				{
				}

				if (!connected)
				{
					String user = parameter.getValues().GetValueOrDefault("user", "");
					String password = parameter.getValues().GetValueOrDefault("password", "");
					password = Encryption.Decrypt(password);
					try
					{

						WNet.AddConnection(path, user, password);
						connected = true;
					}
					catch (Win32Exception e3)
					{
						synchronized(_syncRoot)
						{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
							if(! _requestedPaths.Any(pi=> String.equals(path, pi.Path, StringComparison.OrdinalIgnoreCase) && String.equals(user, pi.User, StringComparison.OrdinalIgnoreCase) && String.equals(password, pi.Password, StringComparison.OrdinalIgnoreCase)))
							{
								PathInfo tempVar = new PathInfo();
								tempVar.setPath(path);
								tempVar.setUser(user);
								tempVar.setPassword(password);
								this._requestedPaths.add(tempVar);
							}
						}
					}


				}

				return connected;

			}
			else
			{
				return true;
			}
		}
		else
		{
			return true;
		}
	}

	public final boolean Request(ResourceParameter parameter, RefObject<Object> resource)
	{
		resource.argvalue = null;
		return IsAvailable(parameter);
	}

	public final void Release(Object resource)
	{

	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Available;

	protected void OnAvailable(EventArgs e)
	{
		if (this.Available != null)
		{
			this.Available(this, e);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	event EventHandler<ProviderRevokeArgs> IResourceProvider.Revoke
//		{
//			add
//			{
//			}
//			remove
//			{
//			}
//		}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}