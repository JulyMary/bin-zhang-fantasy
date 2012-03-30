using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using System.Windows;
using System.ComponentModel;
using System.Xml.Linq;
using Fantasy.IO;
using System.Windows.Media;
using Fantasy.Drawing;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class BusinessScriptAdapter : NotifyPropertyChangedObject, IEntityScript, IWeakEventListener
    {

        public BusinessScriptAdapter(BusinessScript script)
        {
            this.Entity = script;
            this.IsReadOnly = script.IsSystem;
            this.Name = this.Entity.Name;

            _metadata = XElement.Parse(this.Entity.MetaData);

            PropertyChangedEventManager.AddListener(this.Entity, this, "Script");
            PropertyChangedEventManager.AddListener(this.Entity, this, "Name");
            PropertyChangedEventManager.AddListener(this.Entity, this, "EntityState");
            this.DirtyState = this.Entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty;

        }

        public BusinessScript Entity { get; set; }

        private XElement _metadata;

        public string Content
        {
            get
            {
                return this.Entity.Script;
            }
            set
            {
                this.Entity.Script = value;
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
                    string oldExt = this._name != null ? LongPath.GetExtension(this._name) : null;
                    _name = value;

                    this.OnPropertyChanged("Name");
                    
                    string newExt = this._name != null ? LongPath.GetExtension(this._name) : null;
                    if (oldExt != newExt)
                    {
                        this.OnPropertyChanged("Extension");
                        if (!string.IsNullOrEmpty(newExt))
                        {
                            System.Drawing.Icon ico = IconReader.GetFileIcon(newExt, IconReader.IconSize.Small, false);
                            this.Icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(ico.Handle, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                        }
                        else
                        {
                            this.Icon = null;
                        }
                       
                    }

                    this.Entity.Name = this.Name;
                }
            }
        }


        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            switch (((PropertyChangedEventArgs)e).PropertyName)
            {
                case "Script":
                   this.OnPropertyChanged("Content");
                   return true;
                case "Name":
                    this.Name = this.Entity.Name;
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
            BusinessScriptAdapter other = obj as BusinessScriptAdapter;
            return other != null && other.Entity == this.Entity;
        }

        public override int GetHashCode()
        {
            return unchecked(this.Entity.GetHashCode() + 1000);
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

        public string BuildAction
        {
            get
            {
                return this._metadata.Name.LocalName;
            }
            set
            {
                if (this.BuildAction != value)
                {
                    XElement newMetadata = new XElement(value);
                    foreach (XElement child in _metadata.Elements().ToArray())
                    {
                        child.Remove();
                        newMetadata.Add(child);
                    }

                    this._metadata = newMetadata;

                    this.Entity.MetaData = this._metadata.ToString();
                    this.OnPropertyChanged("BuildAction");
                }
            }

        }

        public string CopyTo
        {
            get
            {
                return (string)this._metadata.Element("CopyToOutputDirectory");
            }
            set
            {
                if (this.CopyTo != value)
                {
                    this._metadata.SetElementValue("CopyToOutputDirectory", value);
                    this.Entity.MetaData = this._metadata.ToString();
                    this.OnPropertyChanged("CopyTo");
                }
            }
        }

        public string Generator
        {
            get
            {
                return (string)this._metadata.Element("Generator");
            }
            set
            {
                if (this.Generator != value)
                {
                    this._metadata.SetElementValue("Generator", value);
                    this.Entity.MetaData = this._metadata.ToString();
                    this.OnPropertyChanged("Generator");
                }
            }
        }

        public string CustomToolNamespace
        {
            get
            {
                return (string)this._metadata.Element("CustomToolNamespace");
            }
            set
            {
                if (this.CustomToolNamespace != value)
                {
                    this._metadata.SetElementValue("CustomToolNamespace", value);
                    this.Entity.MetaData = this._metadata.ToString();
                    this.OnPropertyChanged("CustomToolNamespace");
                }
            }
        }

       

        IBusinessEntity IEntityScript.Entity
        {
            get { return this.Entity; }
        }


        #region IEntityScript Members


        public string Extension
        {
            get { return this.Name.Substring(this.Name.LastIndexOf('.') + 1);}
        }

        #endregion

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

        private ImageSource _icon;

        public ImageSource Icon
        {
            get { return _icon; }
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    this.OnPropertyChanged("Icon");
                }
            }
        }
    }
}
