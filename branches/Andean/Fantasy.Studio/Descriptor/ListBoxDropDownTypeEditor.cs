using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Fantasy.Studio.Descriptor
{
    public abstract class ListBoxDropDownTypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                ListBox ctrl = new ListBox();
                ctrl.Dock = DockStyle.Fill;
                ctrl.BorderStyle = BorderStyle.None;
                ctrl.DisplayMember = this.DisplayMember;
                ctrl.Items.AddRange(this.Items);
                ctrl.Height = (ctrl.ItemHeight + 2) * ctrl.Items.Count;
                ctrl.SelectedItem = value;
                ctrl.Click += new EventHandler(ListBox_DoubleClick);
                ctrl.Tag = edSvc;

                edSvc.DropDownControl(ctrl);

                value = ctrl.SelectedItem;
            }
            return value;

        }

        protected abstract object[] Items { get; }

        void ListBox_DoubleClick(object sender, EventArgs e)
        {
            ((IWindowsFormsEditorService)((ListBox)sender).Tag).CloseDropDown();
        }

        public virtual string DisplayMember
        {
            get
            {
                return null;
            }
        }
    }
}
