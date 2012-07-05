using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.CodeDom.Compiler;
using System.Transactions;
using Fantasy.Jobs.Properties;
using System.Text.RegularExpressions;
using System.Reflection;
using Fantasy.ServiceModel;
using System.Data;

namespace Fantasy.Jobs.Management
{
    public class EFJobQueueService : AbstractService, IJobQueue
    {

        FantasyJobsEntities _entities;

        private List<JobMetaData> _unterminates = new List<JobMetaData>();

        public override void InitializeService()
        {
            _entities = new FantasyJobsEntities();
            _entities.Connection.Open();
            _unterminates.AddRange(_entities.Unterminates);
            _unterminates.SortBy(m => m.Id); 
            foreach (JobMetaData job in _unterminates)
            {
                if (job.State == JobState.Running)
                {
                    job.State = JobState.Suspended;
                    this.ApplyChange(job);
                }
            }
            base.InitializeService();
        }

        public override void UninitializeService()
        {
            base.UninitializeService();
            _entities.Connection.Close();
            _entities.Dispose();
        }


        #region IJobQueue Members

        public IEnumerable<JobMetaData> Unterminates
        {
            get
            {
                List<JobMetaData> uns;
                lock (_syncRoot)
                {
                    uns = new List<JobMetaData>(_unterminates);
                }
                return uns;
            }
        }

        public IEnumerable<JobMetaData> Terminates
        {
            get { return this._entities.Terminates; }
        }

        public JobMetaData FindJobMetaDataById(Guid id)
        {
            lock (_syncRoot)
            {
                JobMetaData rs = null;
                int pos = this._unterminates.BinarySearchBy(id, j => j.Id);
                if (pos >= 0)
                {
                    rs = _unterminates[pos];
                }

                if (rs == null)
                {
                    rs = (from job in this.Terminates where job.Id == id select job).SingleOrDefault();
                }

                return rs;
            }
        }



        public JobMetaData CreateJobMetaData()
        {
            return new JobMetaData();
        }

        #endregion

        private object _syncRoot = new object();
        public void ApplyChange(JobMetaData job)
        {
            int action = 0;
            bool changed = false;
            bool added = false;
            lock (_syncRoot)
            {
                JobMetaData old = this.FindJobMetaDataById(job.Id);
                if (old != null)
                {
                    old.State = job.State;
                    old.StartTime = job.StartTime;
                    old.EndTime = job.EndTime;
                    job = old;
                }
                using (IDbTransaction tx = this._entities.Connection.BeginTransaction())
                {
                    
                    switch (job.EntityState)
                    {
                        case System.Data.EntityState.Detached:
                            {
                              
                                
                                if (!job.IsTerminated)
                                {
                                    this._entities.Unterminates.AddObject(job);
                                }
                                else
                                {
                                    this._entities.Terminates.AddObject(job);
                                }
                                added = true;
                                action = 1;
                               
                               
                            }
                            break;

                        case System.Data.EntityState.Added:
                        case System.Data.EntityState.Modified:
                            {

                                if (!job.IsTerminated && job.EntityKey.EntitySetName == "Terminates")
                                {
                                    this._entities.Unterminates.AddObject(job);
                                    this._entities.DeleteObject(job);
                                    this._entities.SaveChanges();
                                    
                                    action = 1;
                                }
                                else if (job.IsTerminated && job.EntityKey.EntitySetName == "Unterminates")
                                {
                                    this._entities.DeleteObject(job);
                                    this._entities.SaveChanges();
                                    this._entities.Terminates.AddObject(job);
                                    action = -1;
                                }
                                changed = true;
                            }
                            break;
                    }
                    if (changed || added)
                    {
                        this._entities.SaveChanges();
                    }
                    if (action == 1)
                    {
                        int pos = this._unterminates.BinarySearchBy(job.Id, j => j.Id); 

                        this._unterminates.Insert(~pos, job);
                    }
                    else if (action == -1)
                    {
                        int pos = this._unterminates.BinarySearchBy(job.Id, j => j.Id);
                        if (pos >= 0)
                        {
                            this._unterminates.RemoveAt(pos);
                        }
                    }
                    tx.Commit();
                }
            }
            if (changed)
            {
                this.OnChanged(new JobQueueEventArgs(job));
            }
            else if (added)
            {
                this.OnAdded(new JobQueueEventArgs(job));
            }

        }
        public IEnumerable<JobMetaData> FindTerminated(out int totalCount, string filter, string[] args, string order, int skip, int take)
        {
            return FindJobMetaDataByFilter(out totalCount, this._entities.Terminates, filter, args, order, skip, take);
        }
        public IEnumerable<JobMetaData> FindUnterminated(out int totalCount, string filter, string[] args, string order, int skip, int take)
        {
            List<JobMetaData> uns;
            lock (_syncRoot)
            {
                uns = new List<JobMetaData>(_unterminates);
            }

            return FindJobMetaDataByFilter(out totalCount, uns.AsQueryable(), filter, args, order, skip, take);
            
        }

        private struct Replacement
        {
            public string Pattern;
            public string Value;
        }

        private IEnumerable<JobMetaData> FindJobMetaDataByFilter(out int totalCount, IQueryable<JobMetaData> data, string filter, string[] args, string order, int skip, int take)
        {
            filter = filter ?? string.Empty;
            order = order ?? string.Empty;
            
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            string source = Properties.Resources.JobMetaDataFilterTemplate;

            StringBuilder vars = new StringBuilder();
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    vars.AppendFormat("var _{0} = {1};", i, args[i]);
                }
            }

            Replacement[] replacements = new Replacement[]
            {
                new Replacement() {Pattern="<%=vars%>", Value= vars.ToString()},
                new Replacement() {Pattern="<%=condition%>", Value=filter},
                new Replacement() {Pattern="<%=enableCondition%>", Value= filter == string.Empty? "//" : string.Empty},
                new Replacement() {Pattern="<%=order%>", Value=order},
                new Replacement() {Pattern="<%=enableOrder%>", Value= order == string.Empty? "//" : string.Empty}
            };
            foreach (Replacement item in replacements)
            {
                source = Regex.Replace(source, item.Pattern, item.Value, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            }

            CompilerParameters cp = new CompilerParameters(new string[] 
            {
                "System.dll", "System.Core.dll", "System.Data.Entity.dll" 
            });
            cp.GenerateInMemory = true;
            cp.GenerateExecutable = false;
            cp.IncludeDebugInformation = false;
            cp.TreatWarningsAsErrors = false;
            cp.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            CompilerResults cr = provider.CompileAssemblyFromSource(cp, source);
            if (!cr.Errors.HasErrors)
            {
                
                IJobMetaDataFilter f = (IJobMetaDataFilter)cr.CompiledAssembly.CreateInstance("Fantasy.Jobs.Temporary.JobMetaDataFilter");

                IEnumerable<JobMetaData> rs = f.Filter(data);
                totalCount = rs.Count();
                if (skip > 0 || take < Int32.MaxValue)
                {
                    rs = new List<JobMetaData>(rs);
                }
                if (skip > 0)
                {
                    rs = rs.Skip(skip);
                }
                if (take < Int32.MaxValue)
                {
                    rs = rs.Take(take);
                }
                return rs;
            }
            else
            {
                throw new JobException(String.Format(Properties.Resources.InvalidFilterText, filter));
            }
           

        }

        public IEnumerable<JobMetaData> FindAll()
        {
            List<JobMetaData> uns;
            lock (_syncRoot)
            {
                uns = new List<JobMetaData>(_unterminates);
            }
            return uns.Union(this._entities.Terminates);
        }

        public event EventHandler<JobQueueEventArgs> Changed;

        protected virtual void OnChanged(JobQueueEventArgs e)
        {
            if (this.Changed != null)
            {
                this.Changed(this, e);
            }
        }

        public event EventHandler<JobQueueEventArgs> Added;

        protected virtual void OnAdded(JobQueueEventArgs e)
        {
            if (this.Added != null)
            {
                this.Added(this, e);
            }
        }

        public void Resume(Guid id)
        {
            JobMetaData meta = this.FindJobMetaDataById(id);
            if (meta != null)
            {
                switch (meta.State)
                {
                   
                    case JobState.UserPaused:
                        meta.State = meta.StartTime != null ? JobState.Suspended : JobState.Unstarted; 
                        this.ApplyChange(meta);
                        break;
                    default:
                        throw new InvalidOperationException(String.Format(Properties.Resources.InvalidJobTransitionText, id,
                            JobState.ToString(meta.State), JobState.ToString(JobState.Suspended)));  
                }
            }
        }

        public void Cancel(Guid id)
        {
            JobMetaData meta = this.FindJobMetaDataById(id);
            if (meta != null)
            {
                if(! meta.IsTerminated)
                {
                    if(meta.State == JobState.Running )
                    {
                        if(this.RequestCancel != null)
                        {
                            this.RequestCancel(this, new JobQueueEventArgs(meta));
                        }
                    }
                    else
                    {
                        meta.State = JobState.Cancelled;
                        meta.EndTime = DateTime.Now;
                        this.ApplyChange(meta);
                    }
                }
                else

                {
                    throw new InvalidOperationException(String.Format(Properties.Resources.InvalidJobTransitionText, id,
                        JobState.ToString(meta.State), JobState.ToString(JobState.Cancelled)));
                }
            }
        }

        public void Suspend(Guid id)
        {
            JobMetaData meta = this.FindJobMetaDataById(id);
            if (meta != null)
            {
                if (!meta.IsTerminated)
                {
                    if (meta.State == JobState.Running)
                    {
                        if (this.RequestSuspend != null)
                        {
                            this.RequestSuspend(this, new JobQueueEventArgs(meta));
                        }
                        
                    }
                    else
                    {
                        meta.State = JobState.Suspended;
                        this.ApplyChange(meta);
                    }
                }
                else
                {
                    throw new InvalidOperationException(String.Format(Properties.Resources.InvalidJobTransitionText, id,
                        JobState.ToString(meta.State), JobState.ToString(JobState.Cancelled)));
                }
            }
        }

        public void UserPause(Guid id)
        {
            JobMetaData meta = this.FindJobMetaDataById(id);
            if (meta != null)
            {
                if (!meta.IsTerminated)
                {
                    if (meta.State == JobState.Running)
                    {
                        if (this.RequestUserPause != null)
                        {
                            this.RequestUserPause(this, new JobQueueEventArgs(meta));
                        }
                    }
                    else
                    {
                        meta.State = JobState.UserPaused;
                        this.ApplyChange(meta);
                    }
                }
                else
                {
                    throw new InvalidOperationException(String.Format(Properties.Resources.InvalidJobTransitionText, id,
                        JobState.ToString(meta.State), JobState.ToString(JobState.UserPaused)));
                }
            }
        }

       

        public event EventHandler<JobQueueEventArgs> RequestCancel;

        public event EventHandler<JobQueueEventArgs> RequestSuspend;

        public event EventHandler<JobQueueEventArgs> RequestUserPause;



       


        public string[] GetAllApplications()
        {
            List<JobMetaData> uns;
            lock (_syncRoot)
            {
                uns = new List<JobMetaData>(_unterminates);
            }
            var q1 = from job in uns select job.Application;
            var q2 = from job in this.Terminates select job.Application;

            var q3 = q1.Distinct().Union(q2.Distinct()).Distinct();
            return q3.ToArray();
        }

        public string[] GetAllUsers()
        {
            List<JobMetaData> uns;
            lock (_syncRoot)
            {
                uns = new List<JobMetaData>(_unterminates);
            }
            var q1 = from job in uns select job.User;
            var q2 = from job in this.Terminates select job.User;

            var q3 = q1.Distinct().Union(q2.Distinct()).Distinct();
            return q3.ToArray();
        }



       

        public bool IsTerminated(Guid id)
        {
            lock (_syncRoot)
            {
                return this._unterminates.BinarySearchBy(id, job => job.Id) < 0;
            }
        }

       
    }


    public interface IJobMetaDataFilter
    {
        IEnumerable<JobMetaData> Filter(IQueryable<JobMetaData> source);
    }
}
