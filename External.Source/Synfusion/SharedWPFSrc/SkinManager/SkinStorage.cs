// <copyright file="SkinStorage.cs" company="Syncfusion">
// Copyright Syncfusion Inc. 2001 - 2009. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>

using System;
using System.Windows;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Syncfusion.Windows.Shared
{
    /// <summary>
    /// Class attaches properties for work with skins.
    /// </summary>
#if SyncfusionFramework4_0
    [System.ComponentModel.DesignTimeVisible(false)]
#endif
    public class SkinStorage : DependencyObject
    {
        #region Constants
        /// <summary>
        /// Default skin name.
        /// </summary>
        private const string DefaultName = "Default";

        private static bool windowflag = false;

        #endregion

        #region Public method

        private static void RemoveDictionaryIfExist(FrameworkElement element, ResourceDictionary dictionary)
        {

            if (element != null)
            {

                for (int i = 0; i < element.Resources.MergedDictionaries.Count; i++)
                {
                    var rdic = element.Resources.MergedDictionaries[i];
                    if (rdic.Source == dictionary.Source)
                    {
                        element.Resources.MergedDictionaries.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        /// <summary>
        /// Gets current skin name from given object.
        /// </summary>
        /// <param name="obj">Given object.</param>
        /// <returns><see cref="String"/> value that represents current skin name of given object.</returns>
        public static string GetVisualStyle(DependencyObject obj)
        {
            return (String)obj.GetValue(VisualStyleProperty);
        }

        /// <summary>
        /// Sets new skin name for given object.
        /// </summary>
        /// <param name="obj">Given object.</param>
        /// <param name="value">New skin name.</param>
        public static void SetVisualStyle(DependencyObject obj, string value)
        {
            obj.SetValue(VisualStyleProperty, value);
            if (obj is ISkinStylePropagator)
            {
                ((ISkinStylePropagator)obj).OnStyleChanged(value);
            }
        }
        #endregion

        #region Dependency Properties

        /// <summary>
        /// Identifies <see cref="Syncfusion.Windows.Shared.SkinStorage.VisualStyleProperty"/> dependency attached property.
        /// </summary>
        public static readonly DependencyProperty VisualStyleProperty = DependencyProperty.RegisterAttached(
            "VisualStyle",
            typeof(string),
            typeof(SkinStorage),
            new FrameworkPropertyMetadata(DefaultName, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnVisualStyleChanged)));
        #endregion

        #region Implementation

        /// <summary>
        /// Called when [visual style changed].
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnVisualStyleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            string visualStyle = e.NewValue as string;
            //obj.SetValue(VisualStyleProperty, visualStyle);
            var rd = new ResourceDictionary();
            string s = GetVisualStyle(obj);

            if ((obj as FrameworkElement) != null)
            {

                if (((obj as FrameworkElement).Parent is Window && !windowflag) || ((obj as FrameworkElement).Parent is ChromelessWindow && !windowflag) || (obj is Window) || (obj is ChromelessWindow) || (obj is UserControl)|| (obj is Page))
                {
                    if (obj is Window || obj is ChromelessWindow)
                    {
                        windowflag = true;
                    }


                    if (visualStyle == "Office2007Black")
                    {
                        try
                        {
                            rd.Source = new Uri("/Syncfusion.Shared.WPF;component/SkinManager/Office2007BlackStyle.xaml", UriKind.RelativeOrAbsolute);
                            RemoveDictionaryIfExist(obj as FrameworkElement, rd);
                            (obj as FrameworkElement).Resources.MergedDictionaries.Add(rd);
                        }
                        catch { }
                    }
                    else if (visualStyle == "Blend")
                    {
                        try
                        {
                            rd.Source = new Uri("/Syncfusion.Shared.WPF;component/SkinManager/BlendStyle.xaml", UriKind.RelativeOrAbsolute);
                            RemoveDictionaryIfExist(obj as FrameworkElement, rd);
                            (obj as FrameworkElement).Resources.MergedDictionaries.Add(rd);
                        }
                        catch { }
                    }
                    else if (visualStyle == "Office2003")
                    {
                        try
                        {
                            rd.Source = new Uri("/Syncfusion.Shared.WPF;component/SkinManager/Office2003Style.xaml", UriKind.RelativeOrAbsolute);
                            RemoveDictionaryIfExist(obj as FrameworkElement, rd);
                            (obj as FrameworkElement).Resources.MergedDictionaries.Add(rd);
                        }
                        catch { }
                    }
                    else if (visualStyle == "Office2007Blue")
                    {
                        try
                        {
                            rd.Source = new Uri("/Syncfusion.Shared.WPF;component/SkinManager/Office2007BlueStyle.xaml", UriKind.RelativeOrAbsolute);
                            RemoveDictionaryIfExist(obj as FrameworkElement, rd);
                            (obj as FrameworkElement).Resources.MergedDictionaries.Add(rd);
                        }
                        catch { }
                    }
                    else if (visualStyle == "Office2007Silver")
                    {
                        try
                        {
                            rd.Source = new Uri("/Syncfusion.Shared.WPF;component/SkinManager/Office2007SilverStyle.xaml", UriKind.RelativeOrAbsolute);
                            RemoveDictionaryIfExist(obj as FrameworkElement, rd);
                            (obj as FrameworkElement).Resources.MergedDictionaries.Add(rd);
                        }
                        catch { }
                    }
                    else if (visualStyle == "SyncOrange")
                    {
                        try
                        {
                            rd.Source = new Uri("/Syncfusion.Shared.WPF;component/SkinManager/SyncOrangeStyle.xaml", UriKind.RelativeOrAbsolute);
                            RemoveDictionaryIfExist(obj as FrameworkElement, rd);
                            (obj as FrameworkElement).Resources.MergedDictionaries.Add(rd);
                        }
                        catch { }
                    }
                    else if (visualStyle == "ShinyRed")
                    {
                        try
                        {
                            rd.Source = new Uri("/Syncfusion.Shared.WPF;component/SkinManager/ShinyRedStyle.xaml", UriKind.RelativeOrAbsolute);
                            RemoveDictionaryIfExist(obj as FrameworkElement, rd);
                            (obj as FrameworkElement).Resources.MergedDictionaries.Add(rd);
                        }
                        catch { }
                    }
                    else if (visualStyle == "ShinyBlue")
                    {
                        try
                        {
                            rd.Source = new Uri("/Syncfusion.Shared.WPF;component/SkinManager/ShinyBlueStyle.xaml", UriKind.RelativeOrAbsolute);
                            RemoveDictionaryIfExist(obj as FrameworkElement, rd);
                            (obj as FrameworkElement).Resources.MergedDictionaries.Add(rd);
                        }
                        catch { }
                    }
                    else if (visualStyle == "Default")
                    {
                        try
                        {
                            rd.Source = new Uri("/Syncfusion.Shared.WPF;component/SkinManager/DefaultStyle.xaml", UriKind.RelativeOrAbsolute);
                            RemoveDictionaryIfExist(obj as FrameworkElement, rd);
                            (obj as FrameworkElement).Resources.MergedDictionaries.Add(rd);
                        }
                        catch { }
                    }
                    else if (visualStyle == "VS2010")
                    {
                        try
                        {
                            rd.Source = new Uri("/Syncfusion.Shared.WPF;component/SkinManager/VS2010Style.xaml", UriKind.RelativeOrAbsolute);
                            RemoveDictionaryIfExist(obj as FrameworkElement, rd);
                            (obj as FrameworkElement).Resources.MergedDictionaries.Add(rd);
                        }
                        catch { }
                    }
                    else if (visualStyle == "Office2010Blue")
                    {
                        try
                        {
                            rd.Source = new Uri("/Syncfusion.Shared.WPF;component/SkinManager/Office2010BlueStyle.xaml", UriKind.RelativeOrAbsolute);
                            RemoveDictionaryIfExist(obj as FrameworkElement, rd);
                            (obj as FrameworkElement).Resources.MergedDictionaries.Add(rd);
                        }
                        catch { }
                    }
                    else if (visualStyle == "Office2010Black")
                    {
                        try
                        {
                            rd.Source = new Uri("/Syncfusion.Shared.WPF;component/SkinManager/Office2010BlackStyle.xaml", UriKind.RelativeOrAbsolute);
                            RemoveDictionaryIfExist(obj as FrameworkElement, rd);
                            (obj as FrameworkElement).Resources.MergedDictionaries.Add(rd);
                        }
                        catch { }
                    }
                    else if (visualStyle == "Office2010Silver")
                    {
                        try
                        {
                            rd.Source = new Uri("/Syncfusion.Shared.WPF;component/SkinManager/Office2010SilverStyle.xaml", UriKind.RelativeOrAbsolute);
                            RemoveDictionaryIfExist(obj as FrameworkElement, rd);
                            (obj as FrameworkElement).Resources.MergedDictionaries.Add(rd);
                        }
                        catch { }
                    }                    
                }

                if (IsApply(obj as FrameworkElement))
                {
                    ApplySkin(obj, GetVisualStyle(obj));
                }
            }


        }

        private static SkinTypeAttribute GetSkinAttribute(FrameworkElement element, string currentskin)
        {
            SkinTypeAttribute skinTypeAttr = null;
            if (element != null)
            {
                System.Reflection.MemberInfo inf = element.GetType();
                var attributes = inf.GetCustomAttributes(true);
                for (int i = 0; i < attributes.Length; i++)
                {
                    var attr = attributes[i] as SkinTypeAttribute;
                    if (attr != null && attr.SkinVisualStyle.ToString() == currentskin)
                    {
                        skinTypeAttr = attr;
                        break;
                    }
                }
            }
            return skinTypeAttr;
        }


        private static SkinTypeAttribute GetSkinAttribute(Control element, string currentskin)
        {
            SkinTypeAttribute skinTypeAttr = null;
            if (element != null)
            {
                System.Reflection.MemberInfo inf = element.GetType();
                var attributes = inf.GetCustomAttributes(true);
                for (int i = 0; i < attributes.Length; i++)
                {
                    var attr = attributes[i] as SkinTypeAttribute;
                    if (attr != null && attr.SkinVisualStyle.ToString() == currentskin)
                    {
                        skinTypeAttr = attr;
                        break;
                    }
                }
            }
            return skinTypeAttr;
        }


        /// <summary>
        /// Applies the skin.
        /// </summary>
        /// <param name="fe">The fe.</param>
        /// <param name="style">The style.</param>
        private static void ApplySkin(DependencyObject obj, string style)
        {
            if ((obj as Visual) != null)
            {
                IEnumerable<DependencyObject> child = VisualUtils.EnumLogicalChildrenOfType(obj, typeof(FrameworkElement));

                foreach (FrameworkElement fe in child)
                {
                    if (fe != null && IsApply(fe))
                    {

                        if (fe is Popup)
                        {
                            if ((fe as Popup).Child as Visual != null)
                            {
                                IEnumerable<Visual> popupChild = VisualUtils.EnumChildrenOfType((fe as Popup).Child as Visual, typeof(FrameworkElement));
                                if (popupChild != null)
                                {
                                    ControlIterate(popupChild, style);
                                }
                            }
                        }
                        if (fe is ScrollViewer && fe is Visual)
                        {
                            if (((fe as ScrollViewer).Content as Visual) != null)
                            {
                                IEnumerable<Visual> scrollChild = VisualUtils.EnumChildrenOfType((fe as ScrollViewer).Content as Visual, typeof(FrameworkElement));
                                if (scrollChild != null)
                                {
                                    ControlIterate(scrollChild, style);
                                }
                            }
                        }

                        OuterControlIterate(fe, style);

                        if (fe.GetType().FullName.Contains("Syncfusion"))
                            SetVisualStyle(fe, style);

                    }
                }


                if ((obj as FrameworkElement) != null)
                {
                    OuterControlIterate((obj as FrameworkElement), style);
                }

            }

        }

        private static void OuterControlIterate(FrameworkElement fe, string style)
        {

            var skinControls1 = GetSkinAttribute(fe, style);

            if (skinControls1 != null)
            {
                var themeName = skinControls1.SkinVisualStyle;
                var xamlName = skinControls1.XamlResource;
                var type = skinControls1.Type;
                var rdict = new ResourceDictionary();
                rdict.Source = new Uri(xamlName, UriKind.RelativeOrAbsolute);
                //rd = MergeDic(rd, style);
                try
                {
                    RemoveDictionaryIfExist(fe, rdict);

                    fe.Resources.MergedDictionaries.Add(rdict);
                }
                catch { }
            }


        }


        private static void ControlIterate(IEnumerable<Visual> fe, string style)
        {

            foreach (FrameworkElement frameSkin in fe)
            {
                //frameSkin.GetType().BaseType.GetConstructor(frameSkin.GetType().BaseType.GetType()).Invoke();
                if (frameSkin != null && IsApply(frameSkin))
                {
                    var skinControls = GetSkinAttribute(frameSkin, style);

                    if (skinControls != null)
                    {
                        var themeName = skinControls.SkinVisualStyle;
                        var xamlName = skinControls.XamlResource;
                        var type = skinControls.Type;
                        var rd = new ResourceDictionary();
                        rd.Source = new Uri(xamlName, UriKind.RelativeOrAbsolute);
                        rd = MergeDic(rd, style);
                        try
                        {
                            RemoveDictionaryIfExist(frameSkin, rd);
                            frameSkin.Resources.MergedDictionaries.Add(rd);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }


        private static bool IsApply(FrameworkElement frameSkin)
        {
            if (!(frameSkin is Panel) && !(frameSkin is Image) && !(frameSkin is Decorator) && !(frameSkin is Shape))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static ResourceDictionary MergeDic(ResourceDictionary rd, String skin)
        {
            ResourceDictionary rd1 = new ResourceDictionary();
            if (skin == "Blend")
            {
                rd1.Source = new Uri(@"/Syncfusion.Shared.WPF;component/SkinManager/BlendStyle.xaml", UriKind.RelativeOrAbsolute);
                rd.MergedDictionaries.Add(rd1);

            }
            if (skin == "Office2007Blue")
            {
                rd1.Source = new Uri(@"/Syncfusion.Shared.WPF;component/SkinManager/Office2007BlueStyle.xaml", UriKind.RelativeOrAbsolute);
                rd.MergedDictionaries.Add(rd1);

            }
            if (skin == "Office2007Black")
            {
                rd1.Source = new Uri(@"/Syncfusion.Shared.WPF;component/SkinManager/Office2007BlackStyle.xaml", UriKind.RelativeOrAbsolute);
                rd.MergedDictionaries.Add(rd1);
            }

            if (skin == "Office2007Silver")
            {
                rd1.Source = new Uri(@"/Syncfusion.Shared.WPF;component/SkinManager/Office2007SilverStyle.xaml", UriKind.RelativeOrAbsolute);
                rd.MergedDictionaries.Add(rd1);

            }
            if (skin == "Office2003")
            {
                rd1.Source = new Uri(@"/Syncfusion.Shared.WPF;component/SkinManager/Office2003Style.xaml", UriKind.RelativeOrAbsolute);
                rd.MergedDictionaries.Add(rd1);
                return rd;
            }
            if (skin == "SyncOrange")
            {
                rd1.Source = new Uri(@"/Syncfusion.Shared.WPF;component/SkinManager/SyncOrangeStyle.xaml", UriKind.RelativeOrAbsolute);
                rd.MergedDictionaries.Add(rd1);

            }
            if (skin == "ShinyRed")
            {
                rd1.Source = new Uri(@"/Syncfusion.Shared.WPF;component/SkinManager/ShinyRedStyle.xaml", UriKind.RelativeOrAbsolute);
                rd.MergedDictionaries.Add(rd1);

            }
            if (skin == "ShinyBlue")
            {
                rd1.Source = new Uri(@"/Syncfusion.Shared.WPF;component/SkinManager/ShinyBlueStyle.xaml", UriKind.RelativeOrAbsolute);
                rd.MergedDictionaries.Add(rd1);
            }
            if (skin == "Default")
            {
                rd1.Source = new Uri(@"/Syncfusion.Shared.WPF;component/SkinManager/DefaultStyle.xaml", UriKind.RelativeOrAbsolute);
                rd.MergedDictionaries.Add(rd1);
            }
            if (skin == "Office2010Blue")
            {
                rd1.Source = new Uri(@"/Syncfusion.Shared.WPF;component/SkinManager/Office2010BlueStyle.xaml", UriKind.RelativeOrAbsolute);
                rd.MergedDictionaries.Add(rd1);
            }
            if (skin == "Office2010Black")
            {
                rd1.Source = new Uri(@"/Syncfusion.Shared.WPF;component/SkinManager/Office2010BlackStyle.xaml", UriKind.RelativeOrAbsolute);
                rd.MergedDictionaries.Add(rd1);
            }
            if (skin == "Office2010Silver")
            {
                rd1.Source = new Uri(@"/Syncfusion.Shared.WPF;component/SkinManager/Office2010SilverStyle.xaml", UriKind.RelativeOrAbsolute);
                rd.MergedDictionaries.Add(rd1);
            }
            return rd;
        }

        #endregion
    }

    public interface ISkinStylePropagator
    {
        void OnStyleChanged(string visualStyle);
    }
}
