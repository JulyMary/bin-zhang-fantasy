using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.BusinessEngine.Security
{
    public class BusinessApplicationACLCreatorOwnerSecurityProvider : ObjectWithSite, ISecurityProvider
    {
        #region ISecurityProvider Members

        private static Guid _creatorOwnerId = new Guid("88127de6-9e01-467a-af5a-6bf90183cd11");

        public BusinessObjectSecurity GetObjectSecurity(ObjectSecurityArgs args)
        {
            if (args.Object.Creator == args.User.Id)
            {


                var query = from participant in args.Application.Data.Participants
                            where participant.Class.Id == args.Object.ClassId
                            from acl in participant.ACLs
                            where acl.Role.Id == _creatorOwnerId
                            select acl.Security;

                return query.FirstOrDefault();
                
               
            }
            return null;

        }

        public BusinessObjectSecurity GetClassSecurity(ClassSecurityArgs args)
        {
            return null;
        }

        #endregion
    }
}
