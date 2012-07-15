package Fantasy.ServiceModel;

import Fantasy.Collections.*;

public final class ClientFactory
{
	private static java.util.HashMap<java.lang.Class, ChannelFactory> _factories = new java.util.HashMap<java.lang.Class, ChannelFactory>();

	private static java.util.HashMap<Key<java.lang.Class, java.lang.Class>, ChannelFactory> _duplexFactories = new java.util.HashMap<Key<java.lang.Class, java.lang.Class>, ChannelFactory>();

	private static java.util.HashMap<java.lang.Class, EndpointAddress> _endpointAddresses = new java.util.HashMap<java.lang.Class, EndpointAddress>();

	private static Object _syncRoot = new Object();

	private static <TChannel> DuplexChannelFactory<TChannel> GetDuplexFactory(java.lang.Class callbackType)
	{
		ChannelFactory rs = null;
		synchronized (_syncRoot)
		{
			Key<java.lang.Class, java.lang.Class> key = new Key<java.lang.Class, java.lang.Class>(TChannel.class, callbackType);

			if (!((rs = _duplexFactories.get(key)) != null))
			{
				ClientSection cs = (ClientSection)System.Configuration.ConfigurationManager.GetSection("system.serviceModel/client");
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
				String q1 = from e in cs.Endpoints.<ChannelEndpointElement>Cast() where e.Contract == TChannel.class.FullName select e.getName();
				String configurationName = q1.Single();

				rs = new DuplexChannelFactory<TChannel>(callbackType, configurationName, null);
				_duplexFactories.put(key, rs);
			}
		}
		return (DuplexChannelFactory<TChannel>)rs;
	}

	private static <TChannel> ChannelFactory<TChannel> GetFactory()
	{
		ChannelFactory rs = null;
		synchronized (_syncRoot)
		{
			if(!((rs = _factories.get(TChannel.class)) != null))
			{
				ClientSection cs = (ClientSection)System.Configuration.ConfigurationManager.GetSection("system.serviceModel/client");
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
				String q1 = from e in cs.Endpoints.<ChannelEndpointElement>Cast() where e.Contract == TChannel.class.FullName select e.getName();
				String configurationName = q1.Single();
				rs = new ChannelFactory<TChannel>(configurationName);
				_factories.put(TChannel.class,rs);
			}
		}

		return (ChannelFactory < TChannel >)rs;
	}


	private static EndpointAddress GetAddress(java.lang.Class t)
	{
		EndpointAddress rs = null;
		synchronized (_syncRoot)
		{
			if (!((rs = _endpointAddresses.get(t)) != null))
			{
				ClientSection cs = (ClientSection)System.Configuration.ConfigurationManager.GetSection("system.serviceModel/client");
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
				var q1 = from e in cs.Endpoints.<ChannelEndpointElement>Cast() where e.Contract == t.FullName select e.Address;

				Uri uri = q1.Single();

//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
				var q2 = from a in ChannelSettings.getDefault().getAddresses() from c in a.Contracts where c == t.FullName select a;
				AddressSetting addr = q2.SingleOrDefault();
				if (addr != null)
				{
					String port = "";

					if (addr.getPort() == -1)
					{
						if (!uri.IsDefaultPort)
						{
							port = uri.Port.toString();
							port = ":" + port;
						}
					}
					else
					{
						port = (new Integer(addr.getPort())).toString();
						port = ":" + port;
					}



					uri = new Uri(String.format("%1$s://%2$s%3$s%4$s", uri.Scheme, addr.getHost(), port, uri.AbsolutePath));
				}
				rs = new EndpointAddress(uri);

			}
		}
		return rs;
	}

//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: public static ClientRef<TChannel> Create<TChannel>(string uri = null)
	public static <TChannel> ClientRef<TChannel> Create(String uri)
	{
		ChannelFactory<TChannel> factory = (ChannelFactory< TChannel>)GetFactory<TChannel>();
		EndpointAddress address = DotNetToJavaStringHelper.isNullOrEmpty(uri) ? GetAddress(TChannel.class) : new EndpointAddress(uri);
		return new ClientRef<TChannel>(factory.CreateChannel(address));
	}




//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: public static ClientRef<TChannel> CreateDuplex<TChannel>(object callback, string uri = null)
	public static <TChannel> ClientRef<TChannel> CreateDuplex(Object callback, String uri)
	{
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		DuplexChannelFactory<TChannel> factory = (DuplexChannelFactory<TChannel>)GetDuplexFactory<TChannel>(callback.getClass());
		EndpointAddress address = DotNetToJavaStringHelper.isNullOrEmpty(uri) ? GetAddress(TChannel.class) : new EndpointAddress(uri);
		return new ClientRef<TChannel>(factory.CreateChannel(new InstanceContext(callback), address));
	}

}