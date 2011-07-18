using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio
{
    public interface IEditingViewBuilder
    {
        IEditingViewContent CreateView(object data);
    }
}
