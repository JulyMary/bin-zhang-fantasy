using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace Fantasy.ServiceModel
{
    public class ClientRef<T> : IDisposable
    {

        internal ClientRef(T client)
        {
            this.Client = client;
        }

       

        public void Dispose()
        {

            if (this.State <= CommunicationState.Opened)
            {
                try
                {
                    ((IChannel)this.Client).Close();
                }
                catch
                {
                }
            }
        }


        public CommunicationState State
        {
            get
            {
                return ((IChannel)this.Client).State;
            }
        }
       

        public T Client { get; private set; }


       
    }
}
