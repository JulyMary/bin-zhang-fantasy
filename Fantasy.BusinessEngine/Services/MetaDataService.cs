using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;

namespace Fantasy.BusinessEngine.Services
{
    public class MetaDataService : DefaultEntityService, IMetaDataService
    {
        #region IMetaDataService Members

        private object _syncRoot = new object();
        private BusinessClass[] _allClasses = null;

        public IEnumerable<BusinessClass> AllClasses
        {
            get 
            {
                LoadAllClasses();
                return _allClasses;
            }
        }

        private void LoadAllClasses()
        {
            if (_allClasses == null)
            {
                lock (_syncRoot)
                {
                    _allClasses = this.DefaultSession.Query<BusinessClass>().OrderBy(c => c.Id).ToArray();
                }
            }
        }

        public BusinessClass FindClass(Guid id)
        {
            LoadAllClasses();
            int n = _allClasses.BinarySearchBy(id, c => c.Id);
            return n >= 0 ? _allClasses[n] : null;
        }


        private BusinessApplication[] _allApplications = null;

        public IEnumerable<BusinessApplication> AllApplications
        {
            get 
            {
                LoadAllApplications();
                return _allApplications;
            }
        }

        private void LoadAllApplications()
        {
            throw new NotImplementedException();
        }

        public BusinessApplication FindApplication(Guid id)
        {
            //int n = _allApplications.BinarySearchBy(id, a => a.Id);
            
            //return n >= 0 ? _allClasses[n] : null;
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
