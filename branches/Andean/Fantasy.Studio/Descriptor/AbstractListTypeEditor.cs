using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms.Design;
using Fantasy.ServiceModel;
using Fantasy.Adaption;

namespace Fantasy.Studio.Descriptor
{
	public abstract class AbstractListTypeEditor : UITypeEditor
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
				ctrl.BorderStyle = BorderStyle.None;
				ctrl.Dock = DockStyle.Fill;
				
				foreach (object data in DataList)
				{
					ctrl.Items.Add(new DataWrapper(data));
					if (data.Equals(value))
					{
						ctrl.SelectedIndex = ctrl.Items.Count - 1;
					}
				}

				ctrl.Click += new EventHandler(ListBoxDoubleClick);
				ctrl.Tag = edSvc;

				edSvc.DropDownControl(ctrl);

				if (ctrl.SelectedItem != null)
				{
					value = ((DataWrapper)ctrl.SelectedItem).Data;
				}
				return value;
			}
			else
			{
				return value;
			}

		}

		protected abstract IEnumerable DataList { get;}

		private void ListBoxDoubleClick(object sender, EventArgs e)
		{
			((IWindowsFormsEditorService)((ListBox)sender).Tag).CloseDropDown();
		}


		private class DataWrapper
		{

			public DataWrapper(object data)
			{
				m_data = data;
			}

			private object m_data;

			public object Data
			{
				get { return m_data; }
			}

			public override string ToString()
			{
				return ServiceManager.Services.GetRequiredService<IAdapterManager>().GetAdapter<string>(Data);
			}
		}
	}
}
