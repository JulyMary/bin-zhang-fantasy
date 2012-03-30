using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;

namespace Fantasy.Studio.Descriptor
{
	public class EnumTypeEditor : UITypeEditor, IObjectWithSite
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            IObjectWithSite ws = context.Instance as IObjectWithSite;
            IServiceProvider site = ws != null ? ws.Site : null;
			if (edSvc != null)
			{
				if (Attribute.IsDefined(value.GetType(), typeof(FlagsAttribute)))
				{
					return EditFlagsValue(edSvc, value, site);
				}
				else
				{
					return EditEnumValue(edSvc, value, site);
				}
			}
			else
			{
				return value;
			}
				

		}

		private object EditFlagsValue(IWindowsFormsEditorService edSvc, object value, IServiceProvider services)
		{
			ListView ctrl = new ListView();
			ctrl.CheckBoxes = true;
			ctrl.View = View.List;
			ctrl.Dock = DockStyle.Fill;
			ctrl.BorderStyle = BorderStyle.None;
            EnumConverter convert = new EnumConverter(value.GetType()) { Site = services };
			foreach (Enum en in Enum.GetValues(value.GetType()))
			{
				if (EnumToInt(en) != 0)
				{
					ListViewItem item = new ListViewItem();
					item.Text = convert.ConvertToString(en);
					item.Tag = en;
					item.Checked = (EnumToInt(en) & EnumToInt((Enum)value)) > 0;
					ctrl.Items.Add(item);
				}
			}

			edSvc.DropDownControl(ctrl);

			int rs = 0;
			foreach (ListViewItem item in ctrl.Items)
			{
				if (item.Checked)
				{
					rs |= EnumToInt((Enum)item.Tag);
				}
			}
			return Enum.Parse(value.GetType(), rs.ToString(), true);
			
		}

		private int EnumToInt(Enum value)
		{
			return Int32.Parse(value.ToString("D"));
		}


		private object EditEnumValue(IWindowsFormsEditorService edSvc, object value, IServiceProvider services)
		{
			ListBox ctrl = new ListBox();
			ctrl.BorderStyle = BorderStyle.None;
			ctrl.Dock = DockStyle.Fill;
            EnumConverter convert = new EnumConverter(value.GetType()) { Site = services };
			foreach (Enum en in Enum.GetValues(value.GetType()))
			{
				ctrl.Items.Add(new EnumWrapper(en, convert.ConvertToString(en)));
				if (value.Equals(en))
				{
					ctrl.SelectedIndex = ctrl.Items.Count - 1;
				}
			}

			ctrl.Click += new EventHandler(ListBox_DoubleClick);
			ctrl.Tag = edSvc;

			edSvc.DropDownControl(ctrl);

			if (ctrl.SelectedItem != null)
			{
				value = ((EnumWrapper)ctrl.SelectedItem).m_vlaue;
			}
			return value;
		}


		void ListBox_DoubleClick(object sender, EventArgs e)
		{
			((IWindowsFormsEditorService)((ListBox)sender).Tag).CloseDropDown();
		}

		private class EnumWrapper
		{
			public EnumWrapper(Enum v, string caption)
			{
				m_vlaue = v;
				m_caption = caption;
			}

			public Enum m_vlaue;

			private string m_caption;

			public override string ToString()
			{
				return m_caption;
			}
		}

        #region IObjectWithSite Members

        public IServiceProvider Site
        {
            get;
            set;
        }

        #endregion

       
    }
}
