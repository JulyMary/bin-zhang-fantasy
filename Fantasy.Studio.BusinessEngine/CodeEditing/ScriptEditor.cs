using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Fantasy.Windows;
using System.ComponentModel;
using Fantasy.Studio.BusinessEngine.CodeEditing;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class ScriptEditor : EntityEditingViewContent
    {

        private string _editingPanelPath = "fantasy/studio/businessengine/scripteditor/panels";

        public override string EditingPanelPath
        {
            get { return _editingPanelPath; }
            
        }


        private string _commandBindingPath = "fantasy/studio/businessengine/scripteditor/commandbindings";

        public override string CommandBindingPath
        {
            get { return _commandBindingPath; }
           
        }

        public override string DocumentName
        {
            get { return this.Data != null ? ((IEntityScript)this.Data).Name : string.Empty; }
        }

        private WeakEventListener _nameListener;

    

        public override void Load(object entity)
        {
            base.Load(entity);
            IEntityScript script = (IEntityScript)entity;

            this.Title = script.Name;


            this._nameListener = new WeakEventListener((type, sender, e) =>
            {
                this.Title = script.Name;
                return true;
            });

            PropertyChangedEventManager.AddListener(((INotifyPropertyChanged)script.Entity), this._nameListener, "Name");

        }

        public override string DocumentType
        {
            get { return "cs"; }
        }

        private ImageSource _icon =  new BitmapImage(new Uri("/Fantasy.Studio.BusinessEngine;component/images/csharpfile.png", UriKind.Relative));
        public override ImageSource Icon
        {
            
            get
            {
                return _icon;
            }
            
        }
    }

}
