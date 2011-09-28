using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine
{
    public class BusinessApplication : BusinessEntity, INamedBusinessEntity
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

        public virtual string FullCodeName
        {
            get
            {
                return this.Package != null ? this.Package.FullCodeName + "." + this.CodeName : this.CodeName;
            }
        }

        public virtual string Script
        {
            get
            {
                return (string)this.GetValue("Script", null);
            }
            set
            {
                this.SetValue("Script", value);
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


        private IObservableList<BusinessApplicationParticipant> _persistedParticipants = new ObservableList<BusinessApplicationParticipant>();
        protected internal virtual IObservableList<BusinessApplicationParticipant> PersistedParticipants
        {
            get { return _persistedParticipants; }
            private set
            {
                if (_persistedParticipants != value)
                {
                    _persistedParticipants = value;
                    if (_participants != null)
                    {
                        _participants.Source = value;
                    }
                }
            }
        }

        private ObservableListView<BusinessApplicationParticipant> _participants;
        public virtual IObservableList<BusinessApplicationParticipant> Participants
        {
            get
            {
                if (this._participants == null)
                {
                    this._participants = new ObservableListView<BusinessApplicationParticipant>(this._persistedParticipants);
                }
                return _participants;
            }
        }

       

        public virtual string FullName
        {
            get
            {
                return this.Package != null && this.Package.Id != BusinessPackage.RootPackageId ? this.Package.Name + "." + this.Name : this.Name;
            }
        }

      
    }
}
