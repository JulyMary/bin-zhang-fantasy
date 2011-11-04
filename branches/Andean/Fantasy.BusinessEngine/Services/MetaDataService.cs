using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public class MetaDataService : DefaultEntityService, IMetaDataService
    {

        #region IMetaDataService Members

        public IEnumerable<BusinessClass> AllClasses
        {
            get { throw new NotImplementedException(); }
        }

        public BusinessClass FindClass(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusinessApplication> AllApplications
        {
            get { throw new NotImplementedException(); }
        }

        public BusinessApplication FindApplication(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusinessUser> AllUsers
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<BusinessRole> AllRoles
        {
            get { throw new NotImplementedException(); }
        }

        public BusinessUser FindUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public BusinessRole FindRole(Guid id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
