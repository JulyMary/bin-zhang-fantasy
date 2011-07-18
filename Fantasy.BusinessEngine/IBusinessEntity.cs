using System;
using System.ComponentModel;
namespace Fantasy.BusinessEngine
{
    public interface IBusinessEntity : IEntity
    {
        Guid Id { get; }

        DateTime CreationTime { get; }
        
        bool IsSystem { get; set; }

        DateTime ModificationTime { get; }

    }
}
