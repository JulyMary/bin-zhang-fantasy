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
            if (this._defaultSession != null)
            {
                this._defaultSession.Close();
            }
            base.UninitializeService();
        }



       
        private ISession _defaultSession = null;
        private ISession DefaultSession
        {
            get
            {
                if (_defaultSession == null)
                {
                    _defaultSession = _sessionFactory.OpenSession();
                }
                return _defaultSession;
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


        private int _updateLevel = 0;

        private object _updateSyncRoot = new object();


        public void BeginUpdate()
        {
            lock (_updateSyncRoot)
            {

                _updateLevel++;

                if (_updateLevel == 1)
                {
                    this.DefaultSession.BeginTransaction();
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
                            this.DefaultSession.Flush();
                            this.DefaultSession.Transaction.Commit();
                            this.OnCommitted(EventArgs.Empty);
                        }
                        else
                        {
                            if (!this.DefaultSession.IsConnected)
                            {
                                this.DefaultSession.Transaction.Rollback();
                            }
                        }
                    }
                    catch
                    {
                        if (!this.DefaultSession.IsConnected)
                        {
                            this.DefaultSession.Transaction.Rollback();
                        }
                        throw;
                    }
                }

            }
        }

        public T Get<T>(object id) where T : IEntity
        {
            return this.DefaultSession.Get<T>(id);
        }

        public void Delete(IEntity entity)
        {
            if (entity.EntityState != EntityState.New)
            {
                this.DefaultSession.Delete(entity);
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
            IDbCommand rs = this.DefaultSession.Connection.CreateCommand();
            if (this.DefaultSession.Transaction != null && this.DefaultSession.Transaction.IsActive)
            {
                this.DefaultSession.Transaction.Enlist(rs);
            }
            return rs;
        }


        public void SaveOrUpdate(IEntity entity)
        {
            this.DefaultSession.SaveOrUpdate(entity);
        }

        public IQueryable<T> Query<T>() where T : IEntity
        {
            return this.DefaultSession.Query<T>();
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
            this._defaultSession.Evict(obj);

        }

        public void Evict(Type type)
        {
            this._defaultSession.Evict(type);
        }

    }
}
