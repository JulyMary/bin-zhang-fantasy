using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using System.Windows;
using System.ComponentModel;
using System.Reflection;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    class RoleNode : NotifyPropertyChangedObject, IWeakEventListener
    {
        public RoleNode (BusinessAssociation entity, string propertyName)
	    {
            this.Entity = entity;
            this.PropertyName = propertyName;
            PropertyChangedEventManager.AddListener(entity, this, propertyName); 
	    }



        public string PropertyName { get; private set; }
        public BusinessAssociation Entity { get; private set; }
        public string Name
        {
            get
            {
                return Invoker.Invoke<string>(this.Entity, this.PropertyName);
            }
        }

        #region IWeakEventListener Members

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            this.OnPropertyChanged("Name");
            return true;
        }

        #endregion
    }

   
    


}
