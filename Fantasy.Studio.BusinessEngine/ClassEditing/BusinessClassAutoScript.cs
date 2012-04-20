using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.BusinessEngine.CodeEditing;
using Fantasy.BusinessEngine;
using Fantasy.Studio.BusinessEngine.Properties;
using System.Windows;
using System.ComponentModel;
using Fantasy.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    public class BusinessClassAutoScript : NotifyPropertyChangedObject, IEntityScript, IWeakEventListener
    {

        public BusinessClassAutoScript(BusinessClass @class)
        {
            this.Entity = @class;
            this.IsReadOnly = true;
            this.Name = this.Entity.CodeName + ".Designer.cs";
            PropertyChangedEventManager.AddListener(this.Entity, this, "AutoScript");
            PropertyChangedEventManager.AddListener(this.Entity, this, "Name");
            PropertyChangedEventManager.AddListener(this.Entity, this, "EntityState");
            this.DirtyState = this.Entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty;
        }

        public BusinessClass Entity { get; set; }

        public string Content
        {
            get
            {
                return this.Entity.AutoScript;
            }
            set
            {
                this.Entity.AutoScript = value;
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

       

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            switch (((PropertyChangedEventArgs)e).PropertyName)
            {
                case "AutoScript":
                   this.OnPropertyChanged("Content");
                   return true;
                case "Name":
                    this.Name = this.Entity.Name + ".Designer.cs";
                    return true;
                case "EntityState":
                    this.DirtyState = this.Entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty;
                    return true;
                default:
                    return false;
            }

        }


        public override bool Equals(object obj)
        {
            BusinessClassAutoScript other = obj as BusinessClassAutoScript;
            return other != null && other.Entity == this.Entity;
        }

        public override int GetHashCode()
        {
            return unchecked(this.Entity.GetHashCode() + 1001);
        }

        private bool _isReadOnly;

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                if (_isReadOnly != value)
                {
                    _isReadOnly = value;
                    this.OnPropertyChanged("IsReadOnly");
                }
            }
        }

       

        IBusinessEntity IEntityScript.Entity
        {
            get { return this.Entity; }
        }



       


        public string Extension
        {
            get { return "cs"; }
        }

        private EditingState _dirtyState;

        public EditingState DirtyState
        {
            get { return _dirtyState; }
            set
            {
                if (_dirtyState != value)
                {
                    _dirtyState = value;
                    this.OnPropertyChanged("DirtyState");
                }
            }
        }

        private ImageSource _icon = new BitmapImage(new Uri("/Fantasy.Studio.BusinessEngine;component/images/csharpfile.png", UriKind.Relative));
        public ImageSource Icon
        {

            get
            {
                return _icon;
            }

        }

       

    }
}
