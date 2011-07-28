using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Fantasy.Studio.Descriptor
{
	public class BooleanTypeEditor : UITypeEditor
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
				
				ctrl.Items.Add(Fantasy.Studio.Descriptor.BooleanConverter.FalseText);
				ctrl.Items.Add(Fantasy.Studio.Descriptor.BooleanConverter.TrueText);
				ctrl.SelectedIndex = (bool)value ? 1 : 0;
				ctrl.Click += new EventHandler(ListBox_DoubleClick);
				ctrl.Tag = edSvc;

				edSvc.DropDownControl(ctrl);

				value = ctrl.SelectedIndex == 1;
			}
			return value;

		}

		void ListBox_DoubleClick(object sender, EventArgs e)
		{
			((IWindowsFormsEditorService)((ListBox)sender).Tag).CloseDropDown();
		}
	}
}
