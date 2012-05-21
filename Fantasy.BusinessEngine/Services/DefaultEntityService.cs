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

            this._sessionFactory = cs.Configuration.BuildSessionFactory();

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
            T rs = Activator.CreateInstance<T>();
            IEntity entity = (IEntity)rs;
            this._createListener.OnCreate(entity);
            return rs;
        }


        public T CreateEntity<T>(Guid id) where T : IEntity
        {
            T rs = Activator.CreateInstance<T>();
            IEntity entity = (IEntity)rs;

            if (rs is IBusinessEntityEx)
            {
                ((IBusinessEntityEx)rs).SetId(id);
            }

            this._createListener.OnCreate(entity);
            return rs;
        }

        public IEntity CreateEntity(Type entityType)
        {
            IEntity rs = (IEntity)Activator.CreateInstance(entityType);
           
            this._createListener.OnCreate(rs);
            return rs;
        }

        public IEntity CreateEntity(Type entityType, Guid id)
        {
            IEntity rs = (IEntity)Activator.CreateInstance(entityType);
            if (rs is IBusinessEntityEx)
            {
                ((IBusinessEntityEx)rs).SetId(id);
            }
            this._createListener.OnCreate(rs);
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
