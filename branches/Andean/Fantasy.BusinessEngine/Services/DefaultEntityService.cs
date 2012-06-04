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
using System.Data;
using NHibernate.Linq;
using System.ComponentModel;

namespace Fantasy.BusinessEngine.Services
{
    public class DefaultEntityService : ServiceBase, IEntityService
    {

        public override void InitializeService()
        {

            INHConfigurationService cs = this.Site.GetRequiredService<INHConfigurationService>();

            this._preDeleteEventListener = new NHPreDeleteEventListener() { Site = this.Site };
            this._postDeleteEventListener = new NHPostDeleteEventListener() { Site = this.Site };
            this._createListener = new EntityCreateEventListener() { Site = this.Site };

            lock (cs.Configuration)
            {

                this._sessionFactory = cs.Configuration.BuildSessionFactory();
            }

            base.InitializeService();
        }

        private NHPreDeleteEventListener _preDeleteEventListener;
        private NHPostDeleteEventListener _postDeleteEventListener;

        public override void UninitializeService()
        {
            if (this._session != null)
            {
                this._session.Close();
            }
            base.UninitializeService();
        }



       
        private ISession _session = null;
        public ISession Session
        {
            get
            {
                if (_session == null)
                {
                    _session = _sessionFactory.OpenSession();
                }
                return _session;
            }
        }

        private ISessionFactory _sessionFactory;
       



        private EntityCreateEventListener _createListener;

        public T CreateEntity<T>() where T : IEntity
        {
            return CreateEntity<T>(null);
        }


        public T CreateEntity<T>(object key) where T : IEntity
        {
            T rs = Activator.CreateInstance<T>();

            EntityCreateEventArgs e = new EntityCreateEventArgs(rs) { Key = key };
         
            this._createListener.OnCreate(e);
            return rs;
        }





       

        public IEntity CreateEntity(Type entityType)
        {
            return this.CreateEntity(entityType, null);
        }

        public IEntity CreateEntity(Type entityType, object key)
        {
            IEntity rs = (IEntity)Activator.CreateInstance(entityType);
            EntityCreateEventArgs e = new EntityCreateEventArgs(rs) { Key = key };
            this._createListener.OnCreate(e);
            return rs;
        }


        private int _updateLevel = 0;

        private object _updateSyncRoot = new object();


        public void BeginUpdate()
        {
            lock (_updateSyncRoot)
            {

                _updateLevel++;

                if (_updateLevel == 1)
                {
                    this.Session.BeginTransaction();
                }

            }
        }


        public void EndUpdate(bool commit)
        {
            lock (_updateSyncRoot)
            {

                _updateLevel--;

                if (_updateLevel == 0)
                {

                    try
                    {

                        CancelEventArgs e = new CancelEventArgs();
                        this.OnCommitting(e);
                        commit = commit & !e.Cancel;
                        if (commit)
                        {
                            this.Session.Flush();
                            this.Session.Transaction.Commit();
                            this.OnCommitted(EventArgs.Empty);
                        }
                        else
                        {
                            if (!this.Session.IsConnected)
                            {
                                this.Session.Transaction.Rollback();
                            }
                        }
                    }
                    catch
                    {
                        if (!this.Session.IsConnected)
                        {
                            this.Session.Transaction.Rollback();
                        }
                        throw;
                    }
                }

            }
        }

        public T Get<T>(object id) where T : IEntity
        {
            return this.Session.Get<T>(id);
        }

        public IEntity Get(Type entityType, object id)
        {
            return (IEntity)this.Session.Get(entityType, id);
        }


        public T Load<T>(object id) where T : IEntity
        {
            return (T)this.Get(typeof(T), id);
        }

        public IEntity Load(Type entityType, object id)
        {
            IEntity rs = this.Get(entityType, id);
            if (rs == null)
            {
                throw new EntityNotFoundException(entityType, id);
            }

            return rs;
        }


        public void Delete(IEntity entity)
        {
            if (entity.EntityState != EntityState.New)
            {
                this.Session.Delete(entity);
            }
            else
            {
                if (!this._preDeleteEventListener.OnPreDelete(entity))
                {
                    this._postDeleteEventListener.OnPostDelete(entity);
                }
            }
        }



        public System.Data.IDbCommand CreateCommand()
        {
            IDbCommand rs = this.Session.Connection.CreateCommand();
            if (this.Session.Transaction != null && this.Session.Transaction.IsActive)
            {
                this.Session.Transaction.Enlist(rs);
            }
            return rs;
        }


        public void SaveOrUpdate(IEntity entity)
        {
            this.Session.SaveOrUpdate(entity);
        }

        public IQueryable<T> Query<T>() where T : IEntity
        {
            return this.Session.Query<T>();
        }


        public event CancelEventHandler Committing;

        protected virtual void OnCommitting(CancelEventArgs e)
        {
            if (this.Committing != null)
            {
                this.Committing(this, e);
            }
        }


        public event EventHandler Committed;

        protected virtual void OnCommitted(EventArgs e)
        {
            if (this.Committed != null)
            {
                this.Committed(this, e);
            }
        }

        public void Evict(object obj)
        {
            this._session.Evict(obj);

        }

        public void Evict(Type type)
        {
            this._session.Evict(type);
        }


       


       

       
    }
}
