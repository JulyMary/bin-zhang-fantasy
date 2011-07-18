using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.ServiceModel;

namespace Fantasy.Studio.BusinessEngine
{
    public class ClassViewBuilder : IEditingViewBuilder
    {
        public IEditingViewContent CreateView(object data)
        {
            return new ClassEditor();
        }
    }
}
