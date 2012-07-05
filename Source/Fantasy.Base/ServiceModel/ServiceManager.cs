using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace Fantasy.ServiceModel
{
    public class ServiceManager : ServiceContainer
    {
        private ServiceManager()
        {

        }

        private static ServiceManager _services = new ServiceManager();

        public static ServiceManager Services
        {
            get
            {
                return _services;
            }
        }

       
    }
}
