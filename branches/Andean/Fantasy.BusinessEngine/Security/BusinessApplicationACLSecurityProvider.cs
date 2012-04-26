using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;
using Fantasy.Collections;

namespace Fantasy.BusinessEngine.Security
{
    public class BusinessApplicationACLSecurityProvider : ObjectWithSite, ISecurityProvider
    {

       

        #region ISecurityProvider Members

        public BusinessObjectSecurity GetObjectSecurity(ObjectSecurityArgs args)
        {
            IObjectModelService oms = this.Site.GetRequiredService<IObjectModelService>();
            BusinessClass @class = oms.FindBusinessClass(args.Object.ClassId);
            return GetClassSecurity(@class, args);

        }

        private long _lastUpdateTime = 0;
        private object _syncRoot = new object();

        private BusinessObjectSecurity GetClassSecurity(BusinessClass @class, SecurityArgs args)
        {
            BusinessObjectSecurity rs;
            lock (this._syncRoot)
            {
                TryRefresh();
                Key<BusinessApplicationData, BusinessUser, BusinessClass> key = new Key<BusinessApplicationData, BusinessUser, BusinessClass>(args.Application.Data, args.User, @class);
                rs = this._cache.GetValueOrDefault(key);
                if (rs == null)
                {
                    rs = BusinessObjectSecurity.Create(@class, null, null, null, null);
                    IBusinessUserRoleService urs = this.Site.GetRequiredService<IBusinessUserRoleService>();
                    BusinessApplicationParticipant participant = args.Application.Data.Participants.SingleOrDefault(p => p.Class == @class);
                    if (participant != null)
                    {

                        var acls = from acl in participant.ACLs
                                    join role in urs.GetRolesForUser(args.User) on acl.Role.Id equals role.Id
                                    select acl;
                        foreach (BusinessApplicationACL acl in acls)
                        {
                            rs |= acl.Security;
                        }
                    }
                }

                this._cache.Add(key, rs);
            }

            return rs;
           
        }


        private void TryRefresh()
        {
           
            ILastUpdateTimestampService us = this.Site.GetRequiredService<ILastUpdateTimestampService>();
            long updateTime = us.GetLastUpdateSeconds("USERROLES");

            if (_lastUpdateTime != updateTime)
            {
               
                _lastUpdateTime = updateTime;

                this._cache.Clear();
            }
        }


        private Dictionary<Key<BusinessApplicationData, BusinessUser, BusinessClass>, BusinessObjectSecurity> _cache = new Dictionary<Key<BusinessApplicationData, BusinessUser, BusinessClass>, BusinessObjectSecurity>(); 

        public BusinessObjectSecurity GetClassSecurity(ClassSecurityArgs args)
        {
            return GetClassSecurity(args.Class, args);
        }

        #endregion
    }
}
