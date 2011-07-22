﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine
{
    public class PackageViewBuilder : IEditingViewBuilder
    {



        #region IEditingViewBuilder Members

        public IEditingViewContent CreateView(object data)
        {
            return new PackageEditor();
        }

        #endregion
    }
}