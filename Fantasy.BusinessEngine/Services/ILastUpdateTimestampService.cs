﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public interface ILastUpdateTimestampService
    {
        long GetLastUpdateSeconds(string name);
    }
}
