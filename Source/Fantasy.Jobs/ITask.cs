﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs
{
    public interface ITask : IObjectWithSite
    {
        bool Execute();        
    }
}
