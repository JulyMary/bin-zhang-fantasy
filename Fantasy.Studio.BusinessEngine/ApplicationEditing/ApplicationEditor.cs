using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class ApplicationEditor :  EntityEditingViewContent
    {

        public ApplicationEditor()
        {

        }

        public override string EditingPanelPath
        {
            get { return "fantasy/studio/businessengine/applicationeditor/panels"; }
        }

        public override string CommandBindingPath
        {
            get { return "fantasy/studio/businessengine/applicationeditor/commandbindings"; }
        }

        public override string DocumentName
        {
            get { return this.Data != null ? ((BusinessApplicationData)this.Data).FullName : string.Empty; }
        }

        public override string DocumentType
        {
            get { return "application"; }
        }

        private ImageSource _icon;
        public override ImageSource Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = new BitmapImage(new Uri("/Fantasy.Studio.BusinessEngine;component/images/application.png", UriKind.Relative));
                }
                return _icon;
            }
        }
    }
}
