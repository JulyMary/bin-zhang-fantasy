using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Threading;
using System.Reflection;
using Fantasy.BusinessEngine.Properties;

namespace Fantasy.BusinessEngine.Services
{
    public class BusinessUserRoleService : ServiceBase, IBusinessUserRoleService
    {

       

        private long _lastUpdateTime = -1;
        private bool _unloading = false;


        public override void UninitializeService()
        {
            this._unloading = true;
            base.UninitializeService();

        }
       

        private List<BusinessUser> _users = new List<BusinessUser>();
        private List<BusinessRole> _roles = new List<BusinessRole>();
        private Dictionary<BusinessUser, List<BusinessRole>> _userRoles = new Dictionary<BusinessUser, List<BusinessRole>>();

        private void TrySyncData()
        {
            if (_unloading) return;

            bool reload = false;
            ILastUpdateTimestampService us = this.Site.GetRequiredService<ILastUpdateTimestampService>();
            long updateTime = us.GetLastUpdateSeconds("USERROLES");

            if (_lastUpdateTime != updateTime)
            {
                reload = true;
                _lastUpdateTime = updateTime;
            }

            if (reload)
            {


                IEntityService es = this.Site.GetRequiredService<IEntityService>();
#pragma warning disable 0618
                Assembly businessDataAssembly = Assembly.LoadWithPartialName(Settings.Default.BusinessDataAssemblyName);
#pragma warning restore 0618
                SyncRoles(es, businessDataAssembly);
                SyncUsers(es, businessDataAssembly);

                
            }
            
        }

        private void SyncUsers(IEntityService es, Assembly businessDataAssembly)
        {
            es.Evict(typeof(BusinessUserData));
            this._userRoles.Clear();
            BusinessUserData[] userDatas = es.Query<BusinessUserData>().ToArray();
            foreach (BusinessUserData ud in userDatas)
            {
                BusinessUser user;
                int pos = _users.BinarySearchBy(ud.Id, u => u.Id);
                if (pos >= 0)
                {
                    user = _users[pos];
                }
                else
                {
                    Type userType;
                    switch (ud.ScriptOptions)
                    {
                        case ScriptOptions.Default:
                            userType = businessDataAssembly.GetType(ud.FullCodeName);
                            break;
                        case ScriptOptions.None:
                            userType = typeof(BusinessUser);
                            break;
                        case ScriptOptions.External:
                            userType = Type.GetType(ud.ExternalType);
                            break;
                        default:
                            throw new Exception("Impossible");

                    }

                    user = (BusinessUser)Activator.CreateInstance(userType);


                    user.Site = this.Site;
                    user.Id = ud.Id;
                    _users.Insert(~pos, user);

                }

                user.Name = ud.Name;
                var roles = from rd in ud.Roles join role in this._roles on rd.Id equals role.Id 
                            select role;
                this._userRoles.Add(user, roles.ToList());
                this._userRoles[user].Add(WellknownRoles.Everyone);
                if (user.Id != _guestId)
                {
                    this._userRoles[user].Add(WellknownRoles.Users);
                }
            }

            var deleted = from user in this._users where !userDatas.Any(u => u.Id == user.Id) select user;
            foreach (BusinessUser user in this._users.ToArray())
            {
                this._users.Remove(user);
            }

            if (this._wellknownUsers == null)
            {
                this._wellknownUsers = new WellknownUsers()
                {
                    Administrator = this.UserById(_administratorId),
                    Guest = this.UserById(_guestId)

                };


            }
        }

        private static Guid _administratorId = new Guid("cee4f357-6985-4beb-bcea-0bcabd232c4a");
        private static Guid _guestId = new Guid("4f2accae-e0ed-4605-adfe-3c1a226a0c0c");

        private void SyncRoles(IEntityService es, Assembly businessDataAssembly)
        {
            es.Evict(typeof(BusinessRole));
            BusinessRoleData[] roleDatas = es.Query<BusinessRoleData>().ToArray();
            foreach (BusinessRoleData rd in roleDatas)
            {
                BusinessRole role;
                int pos = _roles.BinarySearchBy(rd.Id, u => u.Id);
                if (pos >= 0)
                {
                    role = _roles[pos];
                }
                else
                {
                    Type roleType;
                    switch (rd.ScriptOptions)
                    {
                        case ScriptOptions.Default:
                            roleType = businessDataAssembly.GetType(rd.FullCodeName);
                            break;
                        case ScriptOptions.None:
                            roleType = typeof(BusinessRole);
                            break;
                        case ScriptOptions.External:
                            roleType = Type.GetType(rd.ExternalType);
                            break;
                        default:
                            throw new Exception("Impossible");

                    }

                    role = (BusinessRole)Activator.CreateInstance(roleType);


                    role.Site = this.Site;
                    role.Id = rd.Id;
                    _roles.Insert(~pos, role);

                }

                role.Name = rd.Name;
            }

            var deleted = from role in this._roles where !roleDatas.Any(u => u.Id == role.Id) select role;
            foreach (BusinessRole role in this._roles.ToArray())
            {
                this._roles.Remove(role);
            }

            if (this._wellknownRoles == null)
            {
                this._wellknownRoles = new WellknownRoles()
                {
                    Administrators = this.RoleById(new Guid("6d21bc2a-870a-4928-b219-fae9c4c9fd15")),
                    Users = this.RoleById(new Guid("E8700B4B-7911-470E-8E0E-458507D1F51D")),
                    Everyone = this.RoleById(new Guid("A51973B2-7CE8-440E-8A9C-1B48B59B0499"))

                };
            }
        }



        #region IBusinessUserRoleService Members

        public IEnumerable<BusinessUser> Users
        {
            get 
            {
                TrySyncData();
                return this._users; 
            }
        }

        public IEnumerable<BusinessRole> Roles
        {
            get
            {
                TrySyncData();
                {
                    return this._roles;
                }
            }
        }

        public IEnumerable<BusinessRole> GetRolesForUser(BusinessUser user)
        {
            TrySyncData();
            return this._userRoles[user];
        }

        public IEnumerable<BusinessUser> GetUsersForRole(BusinessRole role)
        {
            TrySyncData();
            var rs = from pair in this._userRoles
                     where pair.Value.Contains(role)
                     select pair.Key;
            return rs;
        }

        private WellknownRoles _wellknownRoles;
        public WellknownRoles WellknownRoles
        {
            get
            {
                TrySyncData();
                return _wellknownRoles;
            }
        }


        private WellknownUsers _wellknownUsers;
        public WellknownUsers WellknownUsers
        {
            get
            {
                TrySyncData();
                return _wellknownUsers;
            }
        }

        #endregion


        
    }
}
