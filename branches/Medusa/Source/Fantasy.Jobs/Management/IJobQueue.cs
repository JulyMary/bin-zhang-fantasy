using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Common;
using System.Transactions;
using System.CodeDom.Compiler;
using Fantasy.Jobs.Properties;
using System.Text.RegularExpressions;
using System.ServiceModel;

namespace Fantasy.Jobs.Management
{
    public interface IJobQueue
    {
        IEnumerable<JobMetaData> Unterminates { get; }

        IEnumerable<JobMetaData> Terminates { get; }

        JobMetaData FindJobMetaDataById(Guid id);

        bool IsTerminated(Guid id);

        IEnumerable<JobMetaData> FindTerminated(out int totalCount, string filter, string[] args, string order, int skip, int take);
        IEnumerable<JobMetaData> FindUnterminated(out int totalCount, string filter, string[] args, string order, int skip, int take);

        IEnumerable<JobMetaData> FindAll(); 

        JobMetaData CreateJobMetaData();

        void ApplyChange(JobMetaData job);
        void Resume(Guid id);
        void Cancel(Guid id);
        void Suspend(Guid id);
        void UserPause(Guid id);

        event EventHandler<JobQueueEventArgs> Changed;

        event EventHandler<JobQueueEventArgs> Added;

        event EventHandler<JobQueueEventArgs> RequestCancel;

        event EventHandler<JobQueueEventArgs> RequestSuspend;

        event EventHandler<JobQueueEventArgs> RequestUserPause;


        string[] GetAllApplications();

        string[] GetAllUsers();
    }


    public class JobQueueEventArgs : EventArgs
    {
        public JobQueueEventArgs()
        {

        }

        public JobQueueEventArgs(JobMetaData job)
        {
            this.Job = job;
        }

        public JobMetaData Job { get; private set; }
    }

   

}
