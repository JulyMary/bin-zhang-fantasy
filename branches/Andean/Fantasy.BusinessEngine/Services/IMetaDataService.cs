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

        IEnumerable<BusinessUser> AllUsers { get; }

        IEnumerable<BusinessRole> AllRoles { get; }

        BusinessUser FindUser(Guid id);

        BusinessRole FindRole(Guid id);



        
    }
}
