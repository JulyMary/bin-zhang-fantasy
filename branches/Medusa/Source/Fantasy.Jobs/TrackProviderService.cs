using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Tracking;
using Fantasy.ServiceModel;


namespace Fantasy.Jobs
{
    public class TrackProviderService : AbstractService, ITrackProvider, IJobEngineEventHandler, IDisposable 

    {

        ITrackProvider _provider;

        private Dictionary<string, object> _initValues = new Dictionary<string, object>();



        public override void InitializeService()
        {
            IJobEngine engine = this.Site.GetService<IJobEngine>();
            engine.AddHandler(this);
            base.InitializeService();
        }

        public Guid Id
        {
            get { return _provider != null ? _provider.Id : this.Site.GetService<IJobEngine>().JobId; }
        }

        public string Name
        {
            get { return _provider.Name; }
        }

        public string Category
        {
            get { return _provider != null ? _provider.Category : string.Empty; }
        }

        public object this[string name]
        {
            get
            {
                return _provider != null ? _provider[name] : _initValues.GetValueOrDefault(name, null); 
            }
            set
            {
                if (_provider != null)
                {
                    _provider[name] = value;
                }
                else
                {
                    _initValues[name] = value;
                }
            }
        }

        public string[] PropertyNames
        {
            get { return _provider != null ? _provider.PropertyNames : _initValues.Keys.ToArray(); }
        }

        public TrackFactory Connection
        {
            get { return _provider != null ? _provider.Connection : null; }
        }

        

        #region IJobEngineEventHandler Members

        void IJobEngineEventHandler.HandleStart(IJobEngine sender)
        {
           
        }

        void IJobEngineEventHandler.HandleResume(IJobEngine sender)
        {
           
        }

        void IJobEngineEventHandler.HandleExit(IJobEngine sender, JobExitEventArgs e)
        {
            

           
        }
 
        void IJobEngineEventHandler.HandleLoad(IJobEngine sender)
        {
            Job job = this.Site.GetService<Job>();

            string name = job.GetProperty("name");
            if (string.IsNullOrWhiteSpace(name))
            {
                name = job.TemplateName + DateTime.Now.ToString(); 
            }

            TrackFactory cnnt = new TrackFactory();
            _provider = cnnt.CreateProvider(job.ID, name, "Jobs." + job.TemplateName, _initValues);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
