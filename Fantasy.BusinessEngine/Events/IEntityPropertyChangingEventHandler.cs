﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Events
{
    public interface IEntityPropertyChangingEventHandler
    {
        void Execute(EntityPropertyChangingEventArgs e);
    }
}
