using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Org._12306.Tickets.ServiceModel
{
    public static class ServiceProviderExtension
    {

        public static T GetService<T>(this IServiceProvider services)
        {
            return (T)services.GetService(typeof(T)); 
        }

        public static object GetRequiredService(this IServiceProvider services, Type serviceType)
        {
            object rs = services.GetService(serviceType);
            if (rs == null)
            {
                throw new MissingRequiredServiceException(serviceType); 
            }
            return rs;
        }

        public static T GetRequiredService<T>(this IServiceProvider services)
        {
            return (T)services.GetRequiredService(typeof(T)); 
        }


    }
}
