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

        T Get<T>(object id) where T : IEntity;

        void SaveOrUpdate(IEntity entity);

        void Delete(IEntity entity);

        void BeginUpdate();

        void EndUpdate(bool commit);

        IDbCommand CreateCommand();

        IQueryable<T> Query<T>();

        event CancelEventHandler Committing;

        event EventHandler Committed;


    }


}
