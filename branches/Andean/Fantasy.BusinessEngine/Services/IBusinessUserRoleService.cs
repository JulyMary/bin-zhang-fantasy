using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public interface IBusinessUserRoleService
    {
        IEnumerable<BusinessUser> Users { get;}

        IEnumerable<BusinessRole> Roles { get;}


        IEnumerable<BusinessRole> GetRolesForUser(BusinessUser user);

        IEnumerable<BusinessUser> GetUsersForRole(BusinessRole role);

      

        WellknownRoles WellknownRoles { get; }

        WellknownUsers WellknownUsers { get; }
        

    }
}
