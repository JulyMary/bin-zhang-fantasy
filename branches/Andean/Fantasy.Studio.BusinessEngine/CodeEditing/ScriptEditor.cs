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
using Fantasy.Adaption;

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

        IEntityScript _script; 

        public override void Load(object entity)
        {
            base.Load(entity);
            _script = this.Site.GetRequiredService<IAdapterManager>().GetAdapter<IEntityScript>(entity);

            this.Title = _script.Name;
            this.Icon = _script.Icon;


            this._nameListener = new WeakEventListener((type, sender, e) =>
            {
                PropertyChangedEventArgs args = (PropertyChangedEventArgs)e;
                switch (args.PropertyName)
                {
                    case "Name":
                        this.Title = _script.Name;
                        return true;
                    case "Icon":
                        this.Icon = _script.Icon;
                        return true;
                    default:
                        return false;
                }
                
            });
            if (_script is INotifyPropertyChanged)
            {
                PropertyChangedEventManager.AddListener(((INotifyPropertyChanged)_script), this._nameListener, "Name");
                PropertyChangedEventManager.AddListener(((INotifyPropertyChanged)_script), this._nameListener, "Icon");
            }

        }

      

        public override string DocumentType
        {
            get { return "code"; }
        }

       
    }

}
