﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public interface IBusinessApplicationService
    {

        BusinessApplication Create(Guid id);

        BusinessApplication Create(Type t);
    }
}