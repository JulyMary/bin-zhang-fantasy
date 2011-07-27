using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using NHibernate;
using System.Reflection;
using FluentNHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Event.Default;
using Fantasy.BusinessEngine.Events;

namespace Fantasy.BusinessEngine.Services
{
    public class DefaultEntityService : ServiceBase, IEntityService
    {

        public override void InitializeService()
        {
            this._configuration = new NHibernate.Cfg.Configuration();
            this._configuration.SetProperty("connection.connection_string", this.Site.GetRequiredService<IConnectionStringProvider>().ConnectionString);
            FluentNHibernate.Diagnostics.NullDiagnosticsLogger logger = new FluentNHibernate.Diagnostics.NullDiagnosticsLogger();
            MappingConfiguration mc = new MappingConfiguration(logger);
            mc.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly());

            foreach (Assembly asm in AddIns.AddInTree.Tree.GetTreeNode("fantasy/nhibernate/assemblies").BuildChildItems(this, this.Site))
            {
                mc.FluentMappings.AddFromAssembly(asm);
            }

            mc.Apply(this._configuration);


            this._configuration.EventListeners.PostLoadEventListeners = new IPostLoadEventListener[] { new NHPostLoadEventListener() {Site=this.Site},
                new DefaultPostLoadEventListener() };
            this._configuration.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] { new NHPreInsertEventListener() {Site=this.Site}};
            this._configuration.EventListeners.PostInsertEventListeners = new IPostInsertEventListener[] { new NHPostInsertEventListener() { Site = this.Site } };
            this._configuration.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[] { new NHPreUpdateEventListener() { Site = this.Site } };
            this._configuration.EventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[] { new NHPostUpdateEventListener() { Site = this.Site } };
            this._configuration.EventListeners.PreDeleteEventListeners = new IPreDeleteEventListener[] { new NHPreDeleteEventListener() { Site = this.Site } };
            this._configuration.EventListeners.PostDeleteEventListeners = new IPostDeleteEventListener[] { new NHPostDeleteEventListener() { Site = this.Site } };

            this._createListener = new EntityCreateEventListener() { Site = this.Site };

            
            this._sessionFactory = this._configuration.BuildSessionFactory();

            base.InitializeService();
        }

        public override void UninitializeService()
        {
            if (this._defaultSession != null)
            {
                this._defaultSession.Close();
            }
            base.UninitializeService();
        }

        #region INHSessionProvider Members

        public ISession OpenSession()
        {
            return _sessionFactory != null ? _sessionFactory.OpenSession() : null;
        }
        private ISession _defaultSession = null;
        public ISession DefaultSession
        {
            get
            {
                if (_defaultSession == null)
                {
                    _defaultSession = this.OpenSession();
                }
                return _defaultSession;
            }
        }

        private ISessionFactory _sessionFactory;
        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }

        private NHibernate.Cfg.Configuration _configuration;
        public NHibernate.Cfg.Configuration Configuration
        {
            get { return _configuration; }
        }

        private EntityCreateEventListener _createListener;

        public T CreateEntity<T>() where T : IEntity
        {
            T rs = Activator.CreateInstance<T>();
            IEntity entity = (IEntity)rs;
            this._createListener.OnCreate(entity);
            return rs;
        }

        #endregion
    }
}
