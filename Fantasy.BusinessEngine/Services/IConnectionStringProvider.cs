﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public interface IConnectionStringProvider
    {
        string ConnectionString { get; }
    }
}
