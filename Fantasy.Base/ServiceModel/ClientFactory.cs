using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.ServiceModel.Channels;
using System.Reflection;
using Fantasy.Collections;

namespace Fantasy.ServiceModel
{
    public static class ClientFactory
    {
        private static Dictionary<Type, ChannelFactory> _factories = new Dictionary<Type, ChannelFactory>();

        private static Dictionary<Key<Type, Type>, ChannelFactory> _duplexFactories = new Dictionary<Key<Type, Type>, ChannelFactory>(); 

        private static Dictionary<Type, EndpointAddress> _endpointAddresses = new Dictionary<Type, EndpointAddress>();

        private static object _syncRoot = new object();

        private static DuplexChannelFactory<TChannel> GetDuplexFactory<TChannel>(Type callbackType)
        {
            ChannelFactory rs;
            lock (_syncRoot)
            {
                Key<Type, Type> key = new Key<Type, Type>(typeof(TChannel), callbackType);

                if (!_duplexFactories.TryGetValue(key, out rs))
                {
                    ClientSection cs = (ClientSection)System.Configuration.ConfigurationManager.GetSection("system.serviceModel/client");
                    var q1 = from e in cs.Endpoints.Cast<ChannelEndpointElement>() where e.Contract == typeof(TChannel).FullName select e.Name;
                    string configurationName = q1.Single();

                    rs = new DuplexChannelFactory<TChannel>(callbackType, configurationName, null);
                    _duplexFactories.Add(key, rs);
                }
            }
            return (DuplexChannelFactory<TChannel>)rs;
        }

        private static ChannelFactory<TChannel> GetFactory<TChannel>()
        {
            ChannelFactory rs;
            lock (_syncRoot)
            {
                if(!_factories.TryGetValue(typeof(TChannel), out rs))
                {
                    ClientSection cs = (ClientSection)System.Configuration.ConfigurationManager.GetSection("system.serviceModel/client");
                    var q1 = from e in cs.Endpoints.Cast<ChannelEndpointElement>() where e.Contract == typeof(TChannel).FullName select e.Name;
                    string configurationName = q1.Single();
                    rs = new ChannelFactory<TChannel>(configurationName);
                    _factories.Add(typeof(TChannel),rs);
                }
            }

            return (ChannelFactory < TChannel > )rs;
        }


        private static EndpointAddress GetAddress(Type t)
        {
            EndpointAddress rs = null;
            lock (_syncRoot)
            {
                if (!_endpointAddresses.TryGetValue(t, out rs))
                {
                    ClientSection cs = (ClientSection)System.Configuration.ConfigurationManager.GetSection("system.serviceModel/client");
                    var q1 = from e in cs.Endpoints.Cast<ChannelEndpointElement>() where e.Contract == t.FullName select e.Address;

                    Uri uri = q1.Single();

                    var q2 = from a in ChannelSettings.Default.Addresses
                             from c in a.Contracts
                             where c == t.FullName
                             select a;
                    AddressSetting addr = q2.SingleOrDefault();
                    if (addr != null)
                    {
                        string port = string.Empty;
                       
                        if (addr.Port == -1)
                        {
                            if (!uri.IsDefaultPort)
                            {
                                port = uri.Port.ToString();
                                port = ":" + port;
                            }
                        }
                        else
                        {
                            port = addr.Port.ToString();
                            port = ":" + port;
                        }
                       
                        

                        uri = new Uri(string.Format("{0}://{1}{2}{3}", uri.Scheme, addr.Host, port, uri.AbsolutePath)); 
                    }
                    rs = new EndpointAddress(uri);

                }
            }
            return rs;
        }

        public static ClientRef<TChannel> Create<TChannel>()
        {
            ChannelFactory<TChannel> factory = (ChannelFactory< TChannel>)GetFactory<TChannel>();
            EndpointAddress address = GetAddress(typeof(TChannel));
            return new ClientRef<TChannel>(factory.CreateChannel(address));
        }

        public static ClientRef<TChannel> CreateDuplex<TChannel>(object callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback"); 
            }
            DuplexChannelFactory<TChannel> factory = (DuplexChannelFactory<TChannel>)GetDuplexFactory<TChannel>(callback.GetType());
            EndpointAddress address = GetAddress(typeof(TChannel));
            return new ClientRef<TChannel>( factory.CreateChannel(new InstanceContext(callback), address));
        }

    }
}
