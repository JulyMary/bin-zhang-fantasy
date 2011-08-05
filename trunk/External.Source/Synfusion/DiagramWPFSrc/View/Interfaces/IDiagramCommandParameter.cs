﻿#region Copyright Syncfusion Inc. 2001 - 2011
// Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
namespace Syncfusion.Windows.Diagram
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents common property for Commands.
    /// </summary>
    public interface IDiagramCommandParameter
    {
        SelectionFilter SelectionFilter
        {
            get;
            set;
        }
    }
}