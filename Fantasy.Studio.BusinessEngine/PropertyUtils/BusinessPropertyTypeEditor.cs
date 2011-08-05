using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;

namespace Fantasy.Studio.BusinessEngine
{
    public class BusinessPropertyTypeEditor : UITypeEditor, IObjectWithSite
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            object rs = value;
            BusinessDataTypePikcerModel model = new BusinessDataTypePikcerModel(this.Site);
            try
            {
                BusinessDataTypePicker picker = new BusinessDataTypePicker();
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
