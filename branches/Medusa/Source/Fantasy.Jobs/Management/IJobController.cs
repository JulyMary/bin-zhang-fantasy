using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Fantasy.Jobs.Management
{

    
    public interface IJobController
    {
        
        void StartJob(JobMetaData job);
       
        void Resume(JobMetaData job);
        
        void Cancel(Guid id);
       
        void Suspend(Guid id);
       
        void UserPause(Guid id);



        
        int GetAvailableProcessCount();

       
        bool IsJobProccessExisted(Guid id);

        JobMetaData[] GetRunningJobs();
        
        void RegisterJobEngine(IJobEngine engine);

        void SuspendAll(bool waitForExit);
    }
}
