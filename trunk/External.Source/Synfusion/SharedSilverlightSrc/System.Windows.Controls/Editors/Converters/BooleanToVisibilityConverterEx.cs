﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;

namespace Syncfusion.Windows.Tools.Controls
{
    /// <summary>
    /// This class makes relation between <see cref="bool"/> value and <see cref="Visibility"/> value.
    /// </summary>
    public sealed class BooleanToVisibilityConverterEx : IValueConverter
    {
        #region Constants
        /// <summary>
        /// This value indicates a inverting.  
        /// </summary>
        private string direction = "inverse";

        #endregion

        #region Public methods
        /// <summary>
        /// This method converts <see cref="bool"/> value to  <see cref="Visibility"/>.
        /// </summary>
        /// <param name="value">Value for converting.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">Parameter indicates inverting.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Result converting.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;

            if (value is bool)
            {
                flag = (bool)value;
            }
            else if (value is bool?)
            {
                bool? nullable = (bool?)value;
                flag = nullable.HasValue ? nullable.Value : false;
            }

            if (parameter is string && direction == (string)parameter)
            {
                flag = !flag;
            }

            return flag ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// This method converts <see cref="Visibility"/> value to <see cref="bool"/> value. 
        /// </summary>
        /// <param name="value">Value for converting.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">Parameter indicates inverting.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Result converting.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                bool flag = (Visibility)value == Visibility.Visible;

                if (parameter is string && direction == (string)parameter)
                {
                    flag = !flag;
                }

                return flag;
            }

            return false;
        }
        #endregion
    }
}