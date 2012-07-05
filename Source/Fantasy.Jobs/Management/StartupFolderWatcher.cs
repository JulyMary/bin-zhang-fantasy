using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Threading;
using Fantasy.IO;
using System.Threading.Tasks;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Management
{
    public class StartupFolderWatcher : AbstractService
    {

        private List<FileSystemWatcher> _watchers = new List<FileSystemWatcher>();
        private IJobQueue _jobQueue;

        private Queue<string> _fileQueue = new Queue<string>();
        private object _syncRoot = new object();
        private Thread _addingThread;

        private AutoResetEvent _waitHandler = new AutoResetEvent(false);

        public override void InitializeService()
        {

            _jobQueue = this.Site.GetRequiredService<IJobQueue>();

            Task.Factory.StartNew(() =>
            {
                string[] folders = JobManagerSettings.Default.StartupFolders.Split(';');


                string baseDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                foreach (string folder in folders)
                {
                    string fullPath = Fantasy.IO.LongPath.Combine(baseDir, folder);

                    foreach (string s in Directory.GetFiles(fullPath, "*.xml", SearchOption.TopDirectoryOnly))
                    {
                        _fileQueue.Enqueue(s);
                    }

                    FileSystemWatcher watcher = new FileSystemWatcher(fullPath, "*.xml");
                    watcher.Created += new FileSystemEventHandler(FileWatcherCreated);
                    watcher.EnableRaisingEvents = true;
                    _watchers.Add(watcher);

                    _addingThread = ThreadFactory.CreateThread(this.AddJobs).WithStart();
                   
                    

                }
            });
            base.InitializeService();
        }


        private void AddJobs()
        {
            while (true)
            {
                string file = null;
                
                lock (this._fileQueue)
                {
                    if (this._fileQueue.Count > 0)
                    {

                        file = this._fileQueue.Dequeue();
                    }
                }

                if (file != null)
                {
                    this.StartJob(file);
                }
                else
                {
                    this._waitHandler.WaitOne();
                }
               

            }
        }


        void FileWatcherCreated(object sender, FileSystemEventArgs e)
        {
            lock (this._fileQueue)
            {
                if (!this._fileQueue.Contains(e.FullPath))
                {
                    this._fileQueue.Enqueue(e.FullPath);
                    this._waitHandler.Set();
                }
            }
        }

       

        private void StartJob(string path)
        {
            ILogger logger = this.Site.GetService<ILogger>();
            try
            {

                string xml = this.ReadStartInfo(path);
                if (xml != null)
                {

                    JobMetaData job = _jobQueue.CreateJobMetaData();
                    job.LoadXml(xml);

                    _jobQueue.ApplyChange(job);

                    File.Delete(path);

                    if (logger != null)
                    {
                        logger.LogMessage(LogCategories.Manager, "Load job start info at {0}", path);
                    }
                }


            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception error)
            {
                if (logger != null)
                {
                    logger.LogError(LogCategories.Manager, error, "Invalid job start file {0}", path);
                }

                try
                {

                    File.Delete(path);
                }
                catch
                {
                }
            }
            

        }

        private string ReadStartInfo(string path)
        {
            string rs = null;
            while(rs == null && File.Exists(path))
            {
                try
                {

                    rs = File.ReadAllText(path);
                    
                }
                catch(IOException)
                {

                }
                Thread.Sleep(50);
            }

            return rs;
        }

        public override void UninitializeService()
        {

            base.UninitializeService();
            this._addingThread.Abort();
            foreach (FileSystemWatcher w in this._watchers)
            {
                //w.EnableRaisingEvents = false;
                w.Dispose();
            }
        }
        
    }
}
