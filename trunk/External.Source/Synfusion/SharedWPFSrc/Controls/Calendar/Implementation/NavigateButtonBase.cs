// <copyright file="NavigateButtonBase.cs" company="Syncfusion">
// Copyright Syncfusion Inc. 2001 - 2009. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Syncfusion.Windows.Shared
{
    /// <summary>
    /// Implements the basic functionality required by the navigate button.
    /// </summary>
#if SyncfusionFramework4_0
    [System.ComponentModel.DesignTimeVisible(false)]
#endif
    public class NavigateButtonBase : ButtonBase
    {
        #region Private members
        /// <summary>
        /// Defines corner radius of the button.
        /// </summary>
        private CornerRadius mcornerRadius;
        #endregion

        #region Initialization
        /// <summary>
        /// Initializes static members of the <see cref="NavigateButtonBase"/> class.  It overrides some dependency properties.
        /// </summary>
        static NavigateButtonBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigateButtonBase), new FrameworkPropertyMetadata(typeof(NavigateButtonBase)));
            Border.CornerRadiusProperty.AddOwner(typeof(NavigateButtonBase));
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets corner radius of the button.
        /// </summary>
        /// <value>
        /// Type: <see cref="CornerRadius"/>
        /// </value>
        /// <seealso cref="CornerRadius"/>
        public CornerRadius CornerRadius
        {
            get
            {
                return this.mcornerRadius;
            }

            set
            {
                this.mcornerRadius = value;
            }
        }
        #endregion
    }
}
