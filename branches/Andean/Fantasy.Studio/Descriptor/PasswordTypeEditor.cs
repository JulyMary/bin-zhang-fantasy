using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Fantasy.Studio.Descriptor
{

	public class PasswordTypeEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			if (edSvc != null)
			{
				PasswordTypeEditorControl ctrl = new PasswordTypeEditorControl();
				ctrl.Value = (string)value;
				if (edSvc.ShowDialog(ctrl) == DialogResult.OK)
				{
					value = ctrl.Value;
				}
				
			}
			return value;

		}
	}
}
