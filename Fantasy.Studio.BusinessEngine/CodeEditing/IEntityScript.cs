using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.ComponentModel;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public interface IEntityScript : INotifyPropertyChanged
    {
        string Name { get; }
        string Content { get; set; }
        bool IsReadOnly { get; }
        IBusinessEntity Entity { get; }
        

    }
}
