﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public interface  INamedBusinessEntity
    {
        string Name { get; }

       

        string FullName { get; }

        
    }
}
