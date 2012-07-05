using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using Fantasy.Jobs.Management;
using System.IO;
using Fantasy.XSerialization;
using System.Xml;
using System.Threading.Tasks;
using Fantasy.Jobs.Resources;
using System.Xml.Linq;
using System.Reflection;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs
{
    public class JobEngine : MarshalByRefObject, IJobEngine
    {
        public JobEngine(Guid id)
        {
            _currentEngine = this;
            this.JobId = id;
        }

        private ServiceContainer _serviceContainer = new ServiceContainer();

        private Job _job;

        public override object InitializeLifetimeService()
        {
            //Never expired;
            return null;
        }



        private ManualResetEvent _waitEvent = new ManualResetEvent(false);
        private IJobManager _jobManager;

        private Thread _workThread = null;

        private List<IJobEngineEventHandler> _eventHandlers = new List<IJobEngineEventHandler>();

        private static IJobEngine _currentEngine = null;

        public static IJobEngine CurrentEngine
        {
            get
            {
                return _currentEngine;
            }
        }

        public void Initialize()
        {
           
            _jobManager = new JobManagerAccessor().GetJobManager();

            IJobManagerSettingsReader reader = (IJobManagerSettingsReader)_jobManager.GetService(typeof(IJobManagerSettingsReader));
            _jobDirectory = string.Format(String.Format("{0}\\{1}", reader.GetSetting<string>("JobDirectoryFullPath"), this.JobId));
            if (!Directory.Exists(_jobDirectory))
            {
                Directory.CreateDirectory(_jobDirectory);
            }

            ILogger logger = _jobManager.GetService<ILogger>();
            try
            {
                //Initialize Services from app.config
                List<object> services = new List<object>();
                services.AddRange(AddIn.CreateObjects("jobEngine/engine.services/service"));
                services.AddRange(AddIn.CreateObjects("jobEngine/job.services/service"));
                services.Add(this);
                this._serviceContainer.InitializeServices(_jobManager, services.ToArray());
                //Register to Manager
                IJobController controller = (IJobController)_jobManager.GetService(typeof(IJobController));
                controller.RegisterJobEngine(this);
            }
            catch(Exception error)
            {
                if (logger != null)
                {
                    logger.LogError("Engine", error,"Job {0} initialze failed", this.JobId);  
                }
                throw;
            }
        }



        private void FireEvent(MethodInvoker<IJobEngineEventHandler> method)
        {
            List<IJobEngineEventHandler> expired = new List<IJobEngineEventHandler>();
            Parallel.ForEach(this._eventHandlers.ToArray(), handler =>
            // foreach(IJobEngineEventHandler handler in this._eventHandlers) 
            {
                try
                {
                    method(handler);
                }
                catch
                {
                    _eventHandlers.Remove(handler);
                }
            }
            );


        }

        public void WaitForExit()
        {
            _waitEvent.WaitOne();
            _serviceContainer.UninitializeServices();
        }

        private void OnExit(int exitCode)
        {
            try
            {
                JobExitEventArgs e = new JobExitEventArgs(exitCode);
                FireEvent(delegate(IJobEngineEventHandler handler) { handler.HandleExit(this, e); });
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception error)
            {
                ILogger logger = this.GetService<ILogger>();
                if (logger != null)
                {
                    logger.LogError(LogCategories.Engine, error, "A error occurs when job is exiting.");
                }
            }

            _waitEvent.Set();
        }

        private int _abortState = JobState.Suspended;

        public void Start(JobStartInfo startInfo)
        {

            ThreadStart del = delegate()
            {

                int exitState = JobState.Failed;
                ILogger logger = (ILogger)this.GetService(typeof(ILogger));
                try
                {
                   



                    if (logger != null)
                    {
                        logger.LogMessage(LogCategories.Engine, "Start");

                    }

                    FireEvent(delegate(IJobEngineEventHandler handler) { handler.HandleStart(this); });

                    this._job = new Job();
                    this._job.ID = this.JobId;

                    this._serviceContainer.AddService(_job);
                    _job.Initialize();

                    _job.LoadStartInfo((JobStartInfo)startInfo);

                    FireEvent(delegate(IJobEngineEventHandler handler) { handler.HandleLoad(this); });
                    this.SaveStatus();

                    _job.Execute();
                    exitState = JobState.Succeed;
                    this.SaveStatus();
                }
                
                catch (ThreadAbortException)
                {
                    exitState = _abortState;
                }
                catch (Exception error)
                {
                    if (logger != null)
                    {
                        logger.LogError(LogCategories.Engine, error, "A fatal error occurs, exit excuting.");
                        if (error is ReflectionTypeLoadException)
                        {
                            foreach (Exception child in ((ReflectionTypeLoadException)error).LoaderExceptions)
                            {
                                logger.LogMessage(LogCategories.Engine, "Load exception.");
                                logger.LogError(LogCategories.Engine, child, "Load exception.");
                            }
                        }
                    }
                }
                finally
                {
                    if (logger != null)
                    {
                        logger.LogMessage(LogCategories.Engine, "Exit with code {0}.", JobState.ToString(exitState));
                    }
                    this.OnExit(exitState);
                }
            };

            this.StartWorkThread(del);
        }



        public void Resume(JobStartInfo startInfo)
        {

            IJobStatusStorageService storage = this.GetService<IJobStatusStorageService>();
            if (storage.Exists)
            {

                ThreadStart del = delegate()
                {
                    int exitState = JobState.Failed;


                    ILogger logger = (ILogger)this.GetService(typeof(ILogger));
                    try
                    {
                        
                        if (logger != null)
                        {
                            logger.LogMessage(LogCategories.Engine, "Resume");
                        }

                        FireEvent(delegate(IJobEngineEventHandler handler) { handler.HandleResume(this); });

                        this._job = new Job();

                        this._serviceContainer.AddService(_job);
                        _job.Initialize();
                        Stream stream = this.GetRequiredService<IJobStatusStorageService>().Load();
                        XElement ele = XElement.Load(stream);
                        _job.LoadStatus(ele);

                        FireEvent(delegate(IJobEngineEventHandler handler) { handler.HandleLoad(this); });

                        _job.Execute();
                        exitState = JobState.Succeed;
                        this.SaveStatus();
                    }
                    catch (ThreadAbortException)
                    {
                        exitState = _abortState;
                    }
                    catch (Exception error)
                    {

                        if (logger != null)
                        {
                            logger.LogError(LogCategories.Engine, error, "A fatal error occurs, exit excuting.");
                        }
                    }
                    finally
                    {
                        if (logger != null)
                        {
                            logger.LogMessage(LogCategories.Engine, "Exit with code {0}.", JobState.ToString(exitState));
                        }
                        this.OnExit(exitState);
                    }
                };

                this.StartWorkThread(del);
            }
            else
            {
                this.Start(startInfo);
            }
        }

        private void ResumeEntryPoint(object resumeInfo)
        {
            FireEvent(delegate(IJobEngineEventHandler handler) { handler.HandleResume(this); });
            this.OnExit(JobState.Succeed);
        }

        public void Terminate()
        {
            if (this._workThread != null && this._workThread.IsAlive)
            {
                this._abortState = JobState.Cancelled;
                this._workThread.Abort();
            }
        }

        public void Suspend()
        {
            if (this._workThread != null && this._workThread.IsAlive)
            {
                this._abortState = JobState.Suspended;
                this._workThread.Abort();
            }
        }

        public void UserPause()
        {
            if (this._workThread != null && this._workThread.IsAlive)
            {
                this._abortState = JobState.UserPaused;
                this._workThread.Abort();
            }
        }

        public void Fail()
        {
            if (this._workThread != null && this._workThread.IsAlive)
            {
                this._abortState = JobState.Failed;
                this._workThread.Abort();
            }
        }

        public void Sleep(TimeSpan timeout)
        {
            DateTime timeToWait = DateTime.Now + timeout;
            ResourceParameter res = new ResourceParameter("WaitTime", new { time = timeToWait });
            IResourceService resSvc = this.GetService<IResourceService>();
            if (resSvc != null)
            {
                IResourceHandle handle = resSvc.Request(new ResourceParameter[] { res });
                if (handle != null)
                {
                    handle.Dispose();
                }
                
            }
        }

        private void StartWorkThread(ThreadStart start)
        {
            this._workThread = ThreadFactory.CreateThread(start).WithStart();
           
           
        }

        public void AddHandler(IJobEngineEventHandler handler)
        {
            this._eventHandlers.Add(handler);
        }

        public void RemoveHandler(IJobEngineEventHandler handler)
        {
            this._eventHandlers.Remove(handler);
        }

        public object GetService(Type serviceType)
        {
            return _serviceContainer.GetService(serviceType);
        }

        public Guid JobId { get; private set; }

        #region IJobEngine Members


        private string _jobDirectory = null;
        public string JobDirectory
        {
            get
            {
                if (_jobDirectory == null)
                {

                }
                return _jobDirectory;
            }
        }

        #endregion

        private Exception _executingError;
        private bool _abortingSaved = false;

        public void SaveStatus()
        {
            XElement ele = this._job.SaveStatus();
            MemoryStream stream = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings() { Encoding = Encoding.UTF8, Indent = true, IndentChars = "    ", NamespaceHandling = NamespaceHandling.OmitDuplicates };
            XmlWriter writer = XmlWriter.Create(stream, settings);
            ele.Save(writer);
            writer.Close();
            stream.Seek(0, SeekOrigin.Begin);
            this.GetRequiredService<IJobStatusStorageService>().Save(stream);
            
        }

        public void SaveStatusForError(Exception error)
        {
            
            if (_job != null)
            {
                if (error is ThreadAbortException)
                {
                    if (!_abortingSaved)
                    {
                        _abortingSaved = true;
                        if (!this._job.RuntimeStatus.IsRestoring)
                        {
                            this.SaveStatus();
                        }
                    }
                }
                else if (this._executingError != error)
                {

                    _executingError = error;
                    if (!this._job.RuntimeStatus.IsRestoring)
                    {
                        this.SaveStatus();
                    }

                }
            }
           
        }

    }
}
