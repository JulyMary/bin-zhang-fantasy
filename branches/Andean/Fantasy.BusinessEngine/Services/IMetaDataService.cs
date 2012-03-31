using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public interface IMetaDataService : IEntityService
    {
        IEnumerable<BusinessClass> AllClasses { get; }

        BusinessClass FindClass(Guid id);

        IEnumerable<BusinessApplication> AllApplications { get; }

        BusinessApplication FindApplication(Guid id);

        IEnumerable<BusinessUserData> AllUsers { get; }

        IEnumerable<BusinessRoleData> AllRoles { get; }

        BusinessUserData FindUser(Guid id);

        BusinessRoleData FindRole(Guid id);



        
    }
}
