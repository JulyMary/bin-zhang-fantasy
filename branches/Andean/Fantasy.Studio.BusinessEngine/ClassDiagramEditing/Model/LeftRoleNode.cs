using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using System.Windows;
using System.ComponentModel;
using System.Reflection;
using NHibernate;
using Fantasy.BusinessEngine.Services;
using Fantasy.Utils;
using Fantasy.Studio.BusinessEngine.ExtensionEditing;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    public class LeftRoleNode : MemberNode, IWeakEventListener
    {
        public LeftRoleNode (BusinessAssociation entity)
	    {
            this.Entity = entity;
            this.EditingState = entity.EntityState != EntityState.Clean ? EditingState.Dirty : Studio.EditingState.Clean;
            EntityStateChangedEventManager.AddListener(this.Entity, this);
            PropertyChangedEventManager.AddListener(this.Entity, this, "LeftRoleDisplayOrder");
            PropertyChangedEventManager.AddListener(this.Entity, this, "LeftRoleName");
            PropertyChangedEventManager.AddListener(this.Entity, this, "ExtensionsData");
            this._extensionData = new AssociationLeftRoleExtensionData(entity);
	    }


        private ExtensionData _extensionData;
        public override ExtensionData ExtensionData
        {
            get
            {
                return _extensionData;
            }
        }

        public BusinessAssociation Entity { get; private set; }
        public override string Name
        {
            get
            {
                return this.Entity.LeftRoleName;
            }
            set
            {
                string ocname = UniqueNameGenerator.GetCodeName(this.Name);


                this.Entity.LeftRoleName = value;
                if (ocname == this.CodeName)
                {
                    this.CodeName = UniqueNameGenerator.GetCodeName(this.Name);
                }
               
            }
        }


        public string Category
        {
            get
            {
                Fantasy.BusinessEngine.EntityExtensions.Category cate = this.Entity.LeftExtensions.OfType<Fantasy.BusinessEngine.EntityExtensions.Category>().SingleOrDefault();
                return cate != null ? cate.Value : null;
            }
            set
            {
                Fantasy.BusinessEngine.EntityExtensions.Category cate = this.Entity.LeftExtensions.OfType<Fantasy.BusinessEngine.EntityExtensions.Category>().SingleOrDefault();
                if (cate != null)
                {
                    cate.Value = value;
                }
                else
                {
                    cate = new Fantasy.BusinessEngine.EntityExtensions.Category() { Value = value };
                    this.Entity.LeftExtensions.Add(cate);
                }
            }
        }

        public string CodeName
        {
            get
            {
                return this.Entity.LeftRoleCode; 
            }
            set
            {
                this.Entity.LeftRoleCode = value;
            }
        }

        public bool Navigatable
        {
            get
            {
                return this.Entity.LeftNavigatable; 
            }
            set
            {
                this.Entity.LeftNavigatable = value;
            }
        }

        public override long DisplayOrder
        {
            get
            {
                return this.Entity.LeftRoleDisplayOrder;
            }
            set
            {
                this.Entity.LeftRoleDisplayOrder = value;
            }
        }

        public string Cardinality
        {
            get
            {
                return this.Entity.LeftCardinality;
            }
            set
            {
                this.Entity.LeftCardinality = value;
            }
        }

        public override bool IsSystem
        {
            get { return this.Entity.IsSystem; }
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
                    case "LeftRoleName":
                        this.OnPropertyChanged("Name");
                        break;
                    case "LeftRoleDisplayOrder":
                        this.OnPropertyChanged("DisplayOrder");
                        break;
                }

                return true;
            }
            return false;
        }

        #endregion

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

        
    }

   
    


}
