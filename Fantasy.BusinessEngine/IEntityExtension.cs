﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace Fantasy.BusinessEngine
{
    public interface IEntityExtension : INotifyPropertyChanged
    {
        string Name { get;}

        string Description { get; }

    }
}
