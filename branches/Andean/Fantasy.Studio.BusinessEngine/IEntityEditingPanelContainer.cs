﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.BusinessEngine
{
    public interface IEntityEditingPanelContainer
    {
        IEntityEditingPanel ActivePanel { get; set; }

        ICollection<IEntityEditingPanel> Panels { get; }
    }
}