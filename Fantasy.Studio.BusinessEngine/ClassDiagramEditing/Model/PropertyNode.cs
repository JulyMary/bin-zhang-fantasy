using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.Windows;
using System.ComponentModel;
using System.Windows;
using System.Collections.Specialized;
using NHibernate;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.BusinessEngine.ExtensionEditing;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    public class PropertyNode : MemberNode, IWeakEventListener
    {
        public PropertyNode(BusinessProperty property)
        {
          
            this.Entity = property;
            this.EditingState = property.EntityState != EntityState.Clean ? EditingState.Dirty : Studio.EditingState.Clean;
            EntityStateChangedEventManager.AddListener(this.Entity, this);
            PropertyChangedEventManager.AddListener(this.Entity, this, "DisplayOrder");
            PropertyChangedEventManager.AddListener(this.Entity, this, "Name");
            PropertyChangedEventManager.AddListener(this.Entity, this, "ExtensionsData");
            this._extensionData = new PropertyExtensionData(property);
        }


        private ExtensionData _extensionData;
        public override ExtensionData ExtensionData
        {
            get
            {
                return _extensionData;
            }
        }

        public BusinessProperty Entity { get; private set; }


        public override string Name
        {
            get { return this.Entity.Name; }
            set
            {
                this.Entity.Name = value;
            }
        }

        

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(EntityStateChangedEventManager))
            {
                this.EditingState = this.Entity.EntityState != EntityState.Clean ? EditingState.Dirty : Studio.EditingState.Clean;
                return true;
            }
            else if (managerType == typeof(PropertyChangedEventManager))
            {
                PropertyChangedEventArgs args = (PropertyChangedEventArgs)e;
                switch (args.PropertyName)
                {
                    case "Name":
                        this.OnPropertyChanged("Name");
                        break;
                    case "DisplayOrder":
                        this.OnPropertyChanged("DisplayOrder");
                        break;
                }

                return true;
            }
            return false;
        }

        #endregion

        public override long DisplayOrder
        {
            get
            {
                return this.Entity.DisplayOrder;
            }
            set
            {
                this.Entity.DisplayOrder = value;
            }
        }

        public override void SaveEntity()
        {
            if (this.Entity.EntityState == EntityState.Clean)
            {
                return;
            }

            IEntityService es = this.Site.GetRequiredService<IEntityService>();


            es.BeginUpdate();
            try
            {
                es.SaveOrUpdate(this.Entity);
                es.EndUpdate(true);
               
            }
            catch
            {
                es.EndUpdate(false);
                throw;
            }
        }

        public override bool IsSystem
        {
            get { return this.Entity.IsSystem; }
        }
    }
}
