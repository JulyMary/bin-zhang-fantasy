using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public interface IObjectModelService
    {
        IEnumerable<BusinessClass> AllClasses { get; }

        BusinessClass FindBusinessClass(Guid id);

        BusinessClass FindBusinessClassForType(Type type);

        BusinessClass RootClass { get; }


        string GetImageKey(BusinessClass @class);

        string GetImageKey(BusinessClass @class, Enum state);


        
    }
}
