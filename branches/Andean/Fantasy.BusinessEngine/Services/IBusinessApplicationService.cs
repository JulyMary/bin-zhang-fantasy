using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public interface IBusinessApplicationService
    {
        IEnumerable<BusinessApplication> AllApplications { get; }

        BusinessApplication FindBusinessApplication(Guid id);

        BusinessApplication FindBusinessApplicationForType(Type t);
    }
}
