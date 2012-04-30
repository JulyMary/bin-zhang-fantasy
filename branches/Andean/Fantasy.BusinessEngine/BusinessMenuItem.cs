using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine
{
    public class BusinessMenuItem : BusinessEntity
    {

        public virtual string Name
        {
            get
            {
                return (string)this.GetValue("Name", null);
            }
            set
            {
                this.SetValue("Name", value);
            }
        }

        public virtual Guid? EntryObjectId
        {
            get
            {
                return (Guid?)this.GetValue("EntryObjectId", null);
            }
            set
            {
                this.SetValue("EntryObjectId", value);
            }
        }

        public virtual Guid? ApplicationId
        {
            get
            {
                return (Guid?)this.GetValue("ApplicationId", null);
            }
            set
            {
                this.SetValue("ApplicationId", value);
            }
        }

        protected internal virtual byte[] PersistedIcon
        {
            get
            {
                return (byte[])this.GetValue("PersistedIcon", null);
            }
            set
            {
                this.SetValue("PersistedIcon", value);
            }
        }

        private Image _icon;
        public virtual Image Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                if (this._icon != value)
                {

                    this._icon = value;

                    if (value != null)
                    {
                        MemoryStream stream = new MemoryStream();
                        value.Save(stream, value.RawFormat);

                        this.PersistedIcon = stream.GetBuffer();
                    }
                    else
                    {
                        this.PersistedIcon = null;
                    }
                    this.OnNotifyPropertyChangedPropertyChanged("Icon");

                }
            }
        }

        public virtual BusinessMenuItem Parent
        {
            get
            {
                return (BusinessMenuItem)this.GetValue("Parent", null);
            }
            set
            {
                this.SetValue("Parent", value);
            }
        }

        public virtual string ExternalUrl
        {
            get
            {
                return (string)this.GetValue("ExternalUrl", null);
            }
            set
            {
                this.SetValue("ExternalUrl", value);
            }
        }

        protected internal virtual string PersistedRoles
        {
            get
            {
                return (string)this.GetValue("PersistedRoles", null);
            }
            set
            {
                this.SetValue("PersistedRoles", value);
            }
        }

        private ObservableList<Guid> _roles = new ObservableList<Guid>();
        public virtual IList<Guid> Roles
        {
            get
            {
                return _roles;
            }
        }


        protected override void OnCreate(EventArgs e)
        {
            this._roles.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(RolesCollectionChanged);
            base.OnCreate(e);

        }

        void RolesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.PersistedRoles = string.Join(",", this.Roles);
        }


        protected override void OnLoad(EventArgs e)
        {
            if (this.PersistedIcon != null)
            {
                try
                {
                    Image image = Image.FromStream(new MemoryStream(this.PersistedIcon));
                    this._icon = image;
                }
                catch 
                {
                    
                    
                }
            }

            if (!String.IsNullOrEmpty(this.PersistedRoles))
            {
                foreach (string s in this.PersistedRoles.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    this.Roles.Add(new Guid(s));
                }
            }
            this._roles.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(RolesCollectionChanged);
            base.OnLoad(e);
        }



        private IObservableList<BusinessMenuItem> _persistedChildItems = new ObservableList<BusinessMenuItem>();
        protected internal virtual IObservableList<BusinessMenuItem> PersistedChildItems
        {
            get { return _persistedChildItems; }
            private set
            {
                if (_persistedChildItems != value)
                {
                    _persistedChildItems = value;
                    if (_childItems != null)
                    {
                        _childItems.Source = value;
                    }
                }
            }
        }

        private ObservableListView<BusinessMenuItem> _childItems;
        public virtual IObservableList<BusinessMenuItem> ChildItems
        {
            get
            {
                if (this._childItems == null)
                {
                    this._childItems = new ObservableListView<BusinessMenuItem>(this._persistedChildItems);
                }
                return _childItems;
            }
        }

        public virtual string Description
        {
            get
            {
                return (string)this.GetValue("Description", null);

            }
            set
            {
                this.SetValue("Description", value);
            }
        }

        public virtual long DisplayOrder
        {
            get
            {
                return (long)this.GetValue("DisplayOrder", 0L);
            }
            set
            {
                this.SetValue("DisplayOrder", value);
            }
        }

        public virtual bool IsEnabled
        {
            get
            {
                return (bool)this.GetValue("IsEnabled", true);
            }
            set
            {
                this.SetValue("IsEnabled", value);
            }
        }


        public static readonly Guid RootId = new Guid("92fe50ba-0ee2-4927-8e01-2918153345b1");
       
    }
}
