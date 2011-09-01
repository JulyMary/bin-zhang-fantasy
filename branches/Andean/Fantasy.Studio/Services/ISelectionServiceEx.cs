using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.Collections;

namespace Fantasy.Studio.Services
{
    public interface ISelectionServiceEx : ISelectionService
    {
        ICollection SelectableObjects { get; }

        bool IsReadOnly { get; set; }

        event EventHandler IsReadOnlyChanged;

       
    }
}
