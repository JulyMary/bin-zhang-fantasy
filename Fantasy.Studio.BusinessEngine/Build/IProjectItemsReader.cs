﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public interface IProjectItemsReader
    {

        void Read(ProjectImportOptions options);
    }
}