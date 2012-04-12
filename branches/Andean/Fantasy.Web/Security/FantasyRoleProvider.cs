using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Fantasy.Web.Properties;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using System.Configuration.Provider;

namespace Fantasy.Web.Security
{
    public class FantasyRoleProvider : RoleProvider
    {

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        private string _applictionName = Settings.Default.ApplicationName;
        public override string ApplicationName
        {
            get
            {
                return this._applictionName;
            }
            set
            {
                this._applictionName = value;
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotSupportedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
           
            throw new NotSupportedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotSupportedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotSupportedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            IBusinessUserRoleService svc = ServiceManager.Services.GetRequiredService<IBusinessUserRoleService>();
            BusinessUser user = svc.UserByName(username);
            if (user != null)
            {
                return svc.GetRolesForUser(user).Select(r => r.Name).ToArray();
            }
            else
            {
                throw new ProviderException(String.Format(Resources.UserDoesNotExistMessage, username));
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            IBusinessUserRoleService svc = ServiceManager.Services.GetRequiredService<IBusinessUserRoleService>();
            BusinessRole role = svc.RoleByName(roleName);
            if (role != null)
            {
                return svc.GetUsersForRole(role).Select(u => u.Name).ToArray();
            }
            else
            {
                throw new ProviderException(String.Format(Resources.RoleDoesNotExistMessage, roleName));
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            IBusinessUserRoleService svc = ServiceManager.Services.GetRequiredService<IBusinessUserRoleService>();
            BusinessUser user = svc.UserByName(username);
            if (user == null)
            {
                throw new ProviderException(String.Format(Resources.UserDoesNotExistMessage, username));
            }
            
            BusinessRole role = svc.RoleByName(roleName);
            if (role == null)
            {
                throw new ProviderException(String.Format(Resources.RoleDoesNotExistMessage, roleName));
            }

            return user.IsInRole(role);

        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotSupportedException();
        }

        public override bool RoleExists(string roleName)
        {
            IBusinessUserRoleService svc = ServiceManager.Services.GetRequiredService<IBusinessUserRoleService>();
            return svc.RoleByName(roleName) != null;
        }
    }
}