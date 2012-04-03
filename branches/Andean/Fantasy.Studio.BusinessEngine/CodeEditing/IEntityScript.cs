﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.ComponentModel;
using System.Windows.Media;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public interface IEntityScript : INotifyPropertyChanged
    {
        string Name { get; }
        string Content { get; set; }
        bool IsReadOnly { get; }
        IBusinessEntity Entity { get; }
        string Extension { get; }
        EditingState DirtyState { get; }

        ImageSource Icon { get; }
        
    }
}