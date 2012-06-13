using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Fantasy.Windows;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Security;

namespace Fantasy.Studio.BusinessEngine.ObjectSecurityEditing
{
    /// <summary>
    /// Interaction logic for ObjectSecurityEditor.xaml
    /// </summary>
    public partial class ObjectSecurityEditor : UserControl
    {
        public ObjectSecurityEditor()
        {
            InitializeComponent();
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(ObjectSecurityEditor_DataContextChanged);
        }

        void ObjectSecurityEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                this.SetCanReadValue();
                this.SetCanWriteValue();
            }
        }


        private bool _updating = false;

        public void SaveLayout()
        {
            GridViewLayoutSetting layout = new GridViewLayoutSetting();
            layout.SaveLayout(this.PropertyGridView);
            Properties.Settings.Default.ObjectSecurityPropertyGridViewLayout = layout;

        }


        private bool _lock = false;

        private void SetCanReadValue()
        {
            if (!_lock)
            {
                _lock = true;
                try
                {
                    bool allow = true;
                    bool deny = true;
                    BusinessObjectSecurity sec = (BusinessObjectSecurity)this.DataContext;
                    foreach (BusinessObjectMemberSecurity prop in sec.Properties)
                    {
                        if (prop.CanRead != true)
                        {
                            allow = false;
                        }
                        if (prop.CanRead != false)
                        {
                            deny = false;
                        }
                        if (!deny && !allow)
                        {
                            break;
                        }
                       
                    }

                    if (allow)
                    {
                        this.CanRead = true;
                    }
                    else if (deny)
                    {
                        this.CanRead = false;
                    }
                    else
                    {
                        this.CanRead = null;
                    }
                }
                finally
                {
                    _lock = false;
                }

               
            }


        }

        private void SetCanWriteValue()
        {
            if (!_lock)
            {
                _lock = true;
                try
                {
                    bool allow = true;
                    bool deny = true;
                    BusinessObjectSecurity sec = (BusinessObjectSecurity)this.DataContext;
                    foreach (BusinessObjectMemberSecurity prop in sec.Properties)
                    {
                        if (prop.CanWrite != true)
                        {
                            allow = false;
                        }
                        if (prop.CanWrite != false)
                        {
                            deny = false;
                        }
                        
                        if (!deny && !allow)
                        {
                            break;
                        }
                    }

                    if (allow)
                    {
                        this.CanWrite = true;
                    }
                    else if (deny)
                    {
                        this.CanWrite = false;
                    }
                    else
                    {
                        this.CanWrite = null;
                    }
                }
                finally
                {
                    this._lock = false;
                }
            }

           
        }




        public bool? CanWrite
        {
            get { return (bool?)GetValue(CanWriteProperty); }
            set { SetValue(CanWriteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanWrite.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanWriteProperty =
            DependencyProperty.Register("CanWrite", typeof(bool?), typeof(ObjectSecurityEditor), new UIPropertyMetadata(null, CanWriteChanged));



        public bool? CanRead
        {
            get { return (bool?)GetValue(CanReadProperty); }
            set { SetValue(CanReadProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanRead.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanReadProperty =
            DependencyProperty.Register("CanRead", typeof(bool?), typeof(ObjectSecurityEditor), new UIPropertyMetadata(null, CanReadChanged));




        static void  CanReadChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            ObjectSecurityEditor editor = (ObjectSecurityEditor)d;

            if (!editor._lock)
            {
                editor._lock = true;
                try
                {
                    BusinessObjectSecurity sec = (BusinessObjectSecurity)editor.DataContext;

                    if ((bool?)e.NewValue == true)
                    {
                        foreach (BusinessObjectMemberSecurity prop in sec.Properties)
                        {
                            prop.CanRead = true;
                        }
                    }
                    else if ((bool?)e.NewValue == false)
                    {
                        foreach (BusinessObjectMemberSecurity prop in sec.Properties)
                        {
                            prop.CanRead = false;
                        }
                    }
                }
                finally
                {
                    editor._lock = false;
                }
            }
        }


        static void CanWriteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            ObjectSecurityEditor editor = (ObjectSecurityEditor)d;

            if (!editor._lock)
            {
                editor._lock = true;
                try
                {
                    BusinessObjectSecurity sec = (BusinessObjectSecurity)editor.DataContext;

                    if ((bool?)e.NewValue == true)
                    {
                        foreach (BusinessObjectMemberSecurity prop in sec.Properties)
                        {
                            prop.CanWrite = true;
                        }
                    }
                    else if ((bool?)e.NewValue == false)
                    {
                        foreach (BusinessObjectMemberSecurity prop in sec.Properties)
                        {
                            prop.CanWrite = false;
                        }
                    }
                }
                finally
                {
                    editor._lock = false;
                }
            }
        }


       

        private void PropertyWriteChecked(object sender, RoutedEventArgs e)
        {
            if (!this._updating)
            {
                this._updating = true;
                try
                {
                    CheckBox checkBox = (CheckBox)sender;

                    foreach (BusinessObjectMemberSecurity prop in this.PropertyListView.SelectedItems)
                    {
                        prop.CanWrite = checkBox.IsChecked;
                    }
                }
                finally
                {
                    this._updating = false;
                }
                this.SetCanWriteValue();
            }
        }

        private void PropertyReadChecked(object sender, RoutedEventArgs e)
        {
            if (!this._updating)
            {
                this._updating = true;
                try
                {
                    CheckBox checkBox = (CheckBox)sender;

                    foreach (BusinessObjectMemberSecurity prop in this.PropertyListView.SelectedItems)
                    {
                        prop.CanRead = checkBox.IsChecked;
                    }
                }
                finally
                {
                    this._updating = false;
                }
                this.SetCanReadValue();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ObjectSecurityPropertyGridViewLayout.LoadLayout(this.PropertyGridView);
        }

        private void PropertyListView_SelectAll(object sender, ExecutedRoutedEventArgs e)
        {
            this.PropertyListView.SelectAll();
        }

       
    }
}
