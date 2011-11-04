using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using NHibernate.Event;
using Fantasy.BusinessEngine.Events;
using NHibernate.Event.Default;

namespace Fantasy.BusinessEngine.Services
{
    public class NHConfigurationService : ServiceBase, Fantasy.BusinessEngine.Services.INHConfigurationService
    {
        private NHibernate.Cfg.Configuration _configuration;
        public NHibernate.Cfg.Configuration Configuration
        {
            get { return _configuration; }
        }

       
        
        public override void InitializeService()
        {

            this._configuration = new NHibernate.Cfg.Configuration();
            this._configuration.SetProperty("connection.connection_string", this.Site.GetRequiredService<IConnectionStringProvider>().ConnectionString);


            this._configuration.EventListeners.PostLoadEventListeners = new IPostLoadEventListener[] { new NHPostLoadEventListener() {Site=this.Site},
                new DefaultPostLoadEventListener() };
            this._configuration.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] { new NHPreInsertEventListener() { Site = this.Site } };
            this._configuration.EventListeners.PostInsertEventListeners = new IPostInsertEventListener[] { new NHPostInsertEventListener() { Site = this.Site } };
            this._configuration.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[] { new NHPreUpdateEventListener() { Site = this.Site } };
            this._configuration.EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[] { new NHPostUpdateEventListener() { Site = this.Site } };

            this._configuration.EventListeners.PreDeleteEventListeners = new IPreDeleteEventListener[] {new NHPreDeleteEventListener() { Site = this.Site }};
            this._configuration.EventListeners.PostDeleteEventListeners = new IPostDeleteEventListener[] { new NHPostDeleteEventListener() { Site = this.Site } };
            base.InitializeService();
        }
    }
}
