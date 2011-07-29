using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Fantasy.BusinessEngine.Services
{
    public static class EntityServiceExtension
    {

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
                    session.Flush();
                    if (commit)
                    {
                        session.Transaction.Commit();
                    }
                    else
                    {
                        session.Transaction.Rollback(); 
                    }
                }
                else
                {
                    _updateLevel[session] = level;
                }
            }
        }
    }
}
