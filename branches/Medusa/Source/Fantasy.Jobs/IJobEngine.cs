using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs
{
    [Serializable]
    public class JobExitEventArgs
    {
        public JobExitEventArgs(int state)
        {
            this.ExitState = state;
        }
        public int ExitState { get; private set; }
    }

    public interface IJobEngineEventHandler
    {
        void HandleStart(IJobEngine sender);
        void HandleResume(IJobEngine sender);
        void HandleLoad(IJobEngine sender);
        void HandleExit(IJobEngine sender, JobExitEventArgs e);
    }

    public interface IJobEngine : IServiceProvider
    {
        Guid JobId { get; } 
        string JobDirectory { get; }
        void Start(JobStartInfo startInfo);
        void Resume(JobStartInfo startInfo);
        void Terminate();
        void Suspend();
        void UserPause();
        void Fail();
        void Sleep(TimeSpan timeToSleep);
        void AddHandler(IJobEngineEventHandler handler);
        void RemoveHandler(IJobEngineEventHandler handler);

        void SaveStatusForError(Exception error);
        void SaveStatus();
    }
    
}
