using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Syncfusion.Licensing;
using Syncfusion.Windows.Shared;


namespace  Syncfusion.Windows.Shared
{
#if SyncfusionFramework4_0
    [System.ComponentModel.DesignTimeVisible(false)]
#endif
    /// <summary>
    /// class which has the Gradient items collection
    /// </summary>
    class GradientItemCollection:ItemsControl
    {
        /// <summary>
        /// Gets or sets the gradient item.
        /// </summary>
        /// <value>The gradient item.</value>
        internal GradientStopItem gradientItem { get; set; }

        /// <summary>
        /// Initializes the <see cref="GradientItemCollection"/> class.
        /// </summary>
        static GradientItemCollection()
        {
            EnvironmentTest.ValidateLicense(typeof(GradientItemCollection));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GradientItemCollection), new FrameworkPropertyMetadata(typeof(GradientItemCollection)));
        }
    }
  
}
