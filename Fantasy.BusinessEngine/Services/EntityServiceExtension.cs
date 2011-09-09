using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Data;

namespace Fantasy.BusinessEngine.Services
{
    public static class EntityServiceExtension
    {


        public static IDbCommand CreateCommand(this ISession session)
        {
            IDbCommand rs = session.Connection.CreateCommand();
            if (session.Transaction != null)
            {
                session.Transaction.Enlist(rs);
            }
            return rs;
        }




        private static Dictionary<ISession, int> _updateLevel = new Dictionary<ISession, int>();


        public static void BeginUpdate(this ISession session)
        {
            lock (session)
            {
                int level = _updateLevel.GetValueOrDefault(session, 0);
                level++;
                _updateLevel[session] = level;
                if (level == 1)
                {
                    session.BeginTransaction();
                }
                
            }
        }


        public static void EndUpdate(this ISession session, bool commit)
        {
            lock (session)
            {
                int level = _updateLevel.GetValueOrDefault(session, 0);
                level --;

                if (level == 0)
                {
                    _updateLevel.Remove(session);
                    try
                    {
                        
                        
                        if (commit)
                        {
                            session.Flush();
                            session.Transaction.Commit();
                        }
                        else
                        {
                            if (!session.IsConnected)
                            {
                                session.Transaction.Rollback();
                            }
                        }
                    }
                    catch
                    {
                        if (!session.IsConnected)
                        {
                            session.Transaction.Rollback();
                        }
                        throw;
                    }
                }
                else
                {
                    _updateLevel[session] = level;
                }
            }
        }

        public static BusinessClass GetRootClass(this IEntityService es)
        {
            return es.DefaultSession.Get<BusinessClass>(BusinessClass.RootClassId);
        }

        public static BusinessPackage GetRootPackage(this IEntityService es)
        {
            return es.DefaultSession.Get<BusinessPackage>(BusinessPackage.RootPackageId);  
        }

        public static BusinessAssemblyReferenceGroup GetAssemblyReferenceGroup(this IEntityService es)
        {
            return es.DefaultSession.Get<BusinessAssemblyReferenceGroup>(BusinessAssemblyReferenceGroup.RootId);
        }
    }
}
