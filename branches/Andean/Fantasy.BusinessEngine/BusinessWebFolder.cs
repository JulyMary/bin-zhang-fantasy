using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine
{
    public class BusinessWebFolder : BusinessEntity, IGernateCodeBusinessEntity
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


        public virtual string CodeName
        {
            get
            {
                return (string)this.GetValue("CodeName", null);
            }
            set
            {
                this.SetValue("CodeName", value);
            }
        }

        public virtual BusinessPackage Package
        {
            get
            {
                return (BusinessPackage)this.GetValue("Package", null);
            }
            set
            {
                this.SetValue("Package", value);
            }
        }

        public virtual BusinessWebFolder ParentFolder
        {
            get
            {
                return (BusinessWebFolder)this.GetValue("ParentFolder", null);
            }
            set
            {
                this.SetValue("ParentFolder", value);
            }
        }

        private IObservableList<BusinessWebFolder> _persistedChildFolders = new ObservableList<BusinessWebFolder>();
        protected internal virtual IObservableList<BusinessWebFolder> PersistedChildFolders
        {
            get { return _persistedChildFolders; }
            private set
            {
                if (_persistedChildFolders != value)
                {
                    _persistedChildFolders = value;
                    _childFolders.Source = value;
                }
            }
        }

        private ObservableListView<BusinessWebFolder> _childFolders;
        public virtual IObservableList<BusinessWebFolder> ChildFolders
        {
            get
            {
                if (this._childFolders == null)
                {
                    this._childFolders = new ObservableListView<BusinessWebFolder>(this._persistedChildFolders);
                }
                return _childFolders;
            }
        }

        #region IGernateCodeBusinessEntity Members


        public virtual string FullCodeName
        {
            get 
            {
                string rs = CodeName;
                if (this.ParentFolder != null)
                {
                    rs = this.ParentFolder.FullCodeName + "." + CodeName;
                }
                else if (this.Package != null)
                {
                    rs = this.Package.FullCodeName + "." + CodeName;
                }

                return rs;
            }
        }

        #endregion

        #region INamedBusinessEntity Members


        public virtual string FullName
        {
            get
            {
                string rs = Name;
                if (this.ParentFolder != null)
                {
                    rs = this.ParentFolder.FullName + "." + rs;
                }
                else if (this.Package != null)
                {
                    rs = this.Package.FullName + "." + rs;
                }

                return rs;
            }
        }

        #endregion
    }
}
