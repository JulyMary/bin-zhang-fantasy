using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace Fantasy.Studio.Services
{
    public interface IMonitorSelectionService
    {
        ISelectionService CurrentSelectionService { get; set; }

        event EventHandler SelectionChanged;
    }
}
