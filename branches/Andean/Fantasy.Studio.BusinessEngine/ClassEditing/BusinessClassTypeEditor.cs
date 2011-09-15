using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    public class BusinessClassTypeEditor : UITypeEditor, IObjectWithSite
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            object rs = value;
            BusinessClassPikcerModel model = new BusinessClassPikcerModel(this.Site);
            model.SelectedItemChanging += (sender, e) => {
                if (e.Class != null)
                {
                    if (e.Class.Flatten(c => c.ParentClass).Any(c => c == (BusinessClass)((Fantasy.Studio.Descriptor.CustomTypeDescriptor)context.Instance).Component))
                    {
                        e.Cancel = true;
                    }
                }
            }; 
            try
            {
                BusinessClassPicker picker = new BusinessClassPicker();
                picker.DataContext = model;
                if ((bool)picker.ShowDialog())
                {
                    rs = model.SelectedItem;
                }
            }
            finally
            {
                model.TreeViewModel.Items.Clear();
            }
            return rs;


        }

        public IServiceProvider Site { get; set; }
      
        
    }
}
