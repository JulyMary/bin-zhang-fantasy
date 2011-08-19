using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Fantasy.BusinessEngine.Services
{
    public interface IEntityService
    {
        ISession OpenSession();

        ISession DefaultSession {get;}

        ISessionFactory SessionFactory { get; }

        NHibernate.Cfg.Configuration Configuration { get; }

        T CreateEntity<T>() where T : IEntity;


    }


}
