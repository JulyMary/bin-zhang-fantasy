using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.Windows;
using System.ComponentModel;
using NHibernate;
using Fantasy.BusinessEngine.Services;
using Fantasy.Utils;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    public class RightRoleNode : MemberNode, IWeakEventListener
    {
        public RightRoleNode (BusinessAssociation entity)
	    {
            this.Entity = entity;
            this.EditingState = entity.EntityState != EntityState.Clean ? EditingState.Dirty : Studio.EditingState.Clean;
            EntityStateChangedEventManager.AddListener(this.Entity, this);
            PropertyChangedEventManager.AddListener(this.Entity, this, "RightRoleDisplayOrder");
            PropertyChangedEventManager.AddListener(this.Entity, this, "RightRoleName"); 
	    }

        public BusinessAssociation Entity { get; private set; }
        public override string Name
        {
            get
            {
                return this.Entity.RightRoleName;
            }
            set
            {

                string ocname = UniqueNameGenerator.GetCodeName(this.Name);


                this.Entity.RightRoleName = value;
                if (ocname == this.CodeName)
                {
                    this.CodeName = UniqueNameGenerator.GetCodeName(this.Name);
                }
            }
        }

        public string Cardinality
        {
            get
            {
                return this.Entity.RightCardinality;
            }
            set
            {
                this.Entity.RightCardinality = value;
            }
        }


        public string CodeName
        {
            get
            {
                return this.Entity.RightRoleCode; 
            }
            set
            {
                this.Entity.RightRoleCode = value;
            }
        }

        public bool Navigatable
        {
            get
            {
                return this.Entity.RightNavigatable; 
            }
            set
            {
                this.Entity.RightNavigatable = value;
            }
        }

        public override long DisplayOrder
        {
            get
            {
                return this.Entity.RightRoleDisplayOrder;
            }
            set
            {
                this.Entity.RightRoleDisplayOrder = value;
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
                    case "RightRoleName":
                        this.OnPropertyChanged("Name");
                        break;
                    case "RightRoleDisplayOrder":
                        this.OnPropertyChanged("DisplayOrder");
                        break;
                }

                return true;
            }
            return false;
        }

        #endregion

        public override bool IsSystem
        {
            get { return this.Entity.IsSystem; }
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
    }
}
