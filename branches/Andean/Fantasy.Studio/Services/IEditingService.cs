using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.Services
{
    public interface IEditingService
    {
        IEditingViewContent OpenView(object data);
        bool CloseView(object data, bool force);
    }
}
