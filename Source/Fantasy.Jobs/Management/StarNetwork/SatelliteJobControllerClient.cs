using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ClickView.Jobs.Management.StarNetwork
{
    public class SatelliteJobControllerClient : ClientBase<IJobController>, IJobController  
    {
        #region IJobController Members

        public void StartJob(JobMetaData job)
        {
            this.Channel.StartJob(job);
        }

        public void Resume(JobMetaData job)
        {
            this.Channel.StartJob(job);
        }

        public void Cancel(Guid id)
        {
            this.Channel.Cancel(id);
        }

        public void Suspend(Guid id)
        {
            this.Channel.Suspend(id);
        }

        public void UserPause(Guid id)
        {
            this.Channel.UserPause(id);
        }

        public void RegisterJobEngine(IJobEngine engine)
        {
            throw new InvalidOperationException(); 
        }

        

        #endregion

        #region IJobController Members


        public int GetAvailableProcessCount()
        {
            return this.Channel.GetAvailableProcessCount(); 
        }

        #endregion
    }
}
