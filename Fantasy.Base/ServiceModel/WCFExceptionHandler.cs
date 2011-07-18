using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Fantasy.ServiceModel
{
    public static class WCFExceptionHandler
    {

        private static Type[] _knowns = new Type[] 
        {
            typeof(EndpointNotFoundException),
            typeof(TimeoutException),
            typeof(CommunicationException),
            typeof(ServerTooBusyException),
            typeof(CommunicationObjectAbortedException),
            typeof(CommunicationObjectFaultedException),
            typeof(ObjectDisposedException), 
            typeof(CallbackExpiredException) 
        };

        public static void CatchKnowns(Exception error)
        {
            Type et = error.GetType();

            if (et.IsGenericType && et.GetGenericTypeDefinition() == typeof(FaultException<>))
            {
                et = et.GetGenericArguments()[0];
            }


            if (Array.IndexOf(_knowns, et) < 0)
            {
                throw error;
            }
        }

        public static bool CanCatch(Exception error)
        {
            Type et = error.GetType();

            if (et.IsGenericType && et.GetGenericTypeDefinition() == typeof(FaultException<>))
            {
                et = et.GetGenericArguments()[0];
            }

            return Array.IndexOf(_knowns, et) >= 0;
            
        }
    }
}
