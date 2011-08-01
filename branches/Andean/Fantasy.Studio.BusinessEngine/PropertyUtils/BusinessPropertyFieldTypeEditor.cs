using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine
{
    class BusinessPropertyFieldTypeEditor : UITypeEditor, IObjectWithSite
    {

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public IServiceProvider Site { get; set; }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                ListBox ctrl = new ListBox();
                ctrl.Dock = DockStyle.Fill;
                ctrl.BorderStyle = BorderStyle.None;
                IDDLService ddl = this.Site.GetRequiredService<IDDLService>();
                ctrl.Items.AddRange(ddl.DataTypes);
                ctrl.Height = ctrl.ItemHeight * ddl.DataTypes.Length + 100;
                ctrl.SelectedItem = value;
                ctrl.Click += new EventHandler(ListBox_DoubleClick);
                ctrl.Tag = edSvc;
                edSvc.DropDownControl(ctrl);
                value = ctrl.SelectedItem;
            }
            return value;

        }

        void ListBox_DoubleClick(object sender, EventArgs e)
        {
            ((IWindowsFormsEditorService)((ListBox)sender).Tag).CloseDropDown();
        }
       
    }
}
