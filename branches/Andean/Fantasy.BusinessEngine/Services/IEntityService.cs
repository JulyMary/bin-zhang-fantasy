using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Data;
using System.ComponentModel;

namespace Fantasy.BusinessEngine.Services
{
    public interface IEntityService
    {
        T CreateEntity<T>() where T : IEntity;

       

        IEntity CreateEntity(Type entityType);


        T CreateEntity<T>(object key) where T : IEntity;

        IEntity CreateEntity(Type entityType, object key);

     
        IEntity Get(Type entityType, object id);

        T Get<T>(object id) where T : IEntity;

        IEntity Load(Type entityType, object id);

        T Load<T>(object id) where T : IEntity;



        void SaveOrUpdate(IEntity entity);

        void Delete(IEntity entity);

        void BeginUpdate();

        void EndUpdate(bool commit);

        IDbCommand CreateCommand();

        IQueryable<T> Query<T>() where T : IEntity;

        event CancelEventHandler Committing;

        event EventHandler Committed;

        void Evict(object obj);
      
        void Evict(Type type);

        ISession Session { get; }



       
    }


}
