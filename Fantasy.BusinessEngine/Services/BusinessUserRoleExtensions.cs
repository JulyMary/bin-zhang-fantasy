using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public static class BusinessUserRoleExtensions
    {
        public static BusinessUser UserByName(this IBusinessUserRoleService svc, string name)
        {
            return svc.Users.SingleOrDefault(u => u.Name == name);
        }

        public static BusinessUser UserById(this IBusinessUserRoleService svc, Guid id)
        {
            return svc.Users.SingleOrDefault(u => u.Id == id);
        }

        public static BusinessRole RoleByName(this IBusinessUserRoleService svc, string name)
        {
            return svc.Roles.SingleOrDefault(r => r.Name == name);
        }

        public static BusinessRole RoleById(this IBusinessUserRoleService svc, Guid id)
        {
            return svc.Roles.SingleOrDefault(r => r.Id == id);
        }

        public static bool IsInRole(this BusinessUser user, BusinessRole role)
        {
            IBusinessUserRoleService svc = user.Site.GetRequiredService<IBusinessUserRoleService>();
            return svc.GetRolesForUser(user).Any(r => r == role);
        }

        
    }
}
