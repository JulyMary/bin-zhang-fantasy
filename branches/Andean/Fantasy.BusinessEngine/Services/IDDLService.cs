﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public interface IDDLService
    {
        string[] DataTypes {get;}

        string[] Schemas { get; }

        string[] TableSpaces { get; }

        void CreateClassTable(BusinessClass @class);

    }
}
