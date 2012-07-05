using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Threading.Tasks;
using System.Threading;

namespace Fantasy.Jobs.Resources
{
    public class SatelliteGlobalMutexService : AbstractService, IGlobalMutexService
    {
        public bool IsAvaiable(string key)
        {
            bool rs = false;
            using (ClientRef<IGlobalMutexService> svc = ClientFactory.Create<IGlobalMutexService>())
            {
                try
                {
                    rs = svc.Client.IsAvaiable(key);
                }
                catch (Exception error)
                {
                    if (!WCFExceptionHandler.CanCatch(error))
                    {
                        throw;
                    }
                }
            }
            return rs;
        }

        public bool Request(string key, TimeSpan timeout)
        {
            bool rs = false;
            using (ClientRef<IGlobalMutexService> svc = ClientFactory.Create<IGlobalMutexService>())
            {
                try
                {
                    rs = svc.Client.Request(key, timeout);
                }
                catch (Exception error)
                {
                    if (!WCFExceptionHandler.CanCatch(error))
                    {
                        throw;
                    }
                }
            }
            return rs;
        }

        public void Release(string key)
        {
            Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        using (ClientRef<IGlobalMutexService> svc = ClientFactory.Create<IGlobalMutexService>())
                        {
                            try
                            {
                                svc.Client.Release(key);
                                break;
                            }
                            catch (Exception error)
                            {
                                if (!WCFExceptionHandler.CanCatch(error))
                                {
                                    throw;
                                }
                            }
                        }

                        Thread.Sleep(60 * 1000);
                    }
                });
        }

    }
}
