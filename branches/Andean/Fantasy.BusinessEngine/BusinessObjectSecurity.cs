using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Collections.ObjectModel;
using Fantasy.Windows;
using System.Xml.Linq;

namespace Fantasy.BusinessEngine
{
    [XSerializable("object", NamespaceUri=Consts.SecurityNamespace)] 
    public class BusinessObjectSecurity : NotifyPropertyChangedObject
    {
        public BusinessObjectSecurity()
        {
            ObservableCollection<BusinessObjectMemberSecurity> properties = new ObservableCollection<BusinessObjectMemberSecurity>();
            properties.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PropertiesChanged);
            this.Properties = properties;

        }

        void PropertiesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnChanged(EventArgs.Empty);

            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                if (e.NewItems != null)
                {
                    foreach (BusinessObjectMemberSecurity ps in e.NewItems)
                    {
                        ps.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(PropertySecurityChanged);
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (BusinessObjectMemberSecurity ps in e.OldItems)
                    {
                        ps.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(PropertySecurityChanged);
                    }
                }
            }

        }

        void PropertySecurityChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnChanged(EventArgs.Empty);
        }


        [XAttribute("access")]
        private BusinessObjectAccess? _objectAccess;
         
        public BusinessObjectAccess? ObjectAccess
        {
            get { return _objectAccess; }
            set
            {
                if (_objectAccess != value)
                {
                    _objectAccess = value;
                    this.OnPropertyChanged("ObjectAccess");
                    this.OnChanged(EventArgs.Empty);
                }
            }
        }

        [XArray,
        XArrayItem(Name = "property", Type=typeof(BusinessObjectMemberSecurity))]
        public virtual IList<BusinessObjectMemberSecurity> Properties { get; protected set; }


        public virtual BusinessObjectAccess? this[BusinessProperty property]
        {
            get
            {
                return this.GetOrCreatePropertySecurity(property).PropertyAccess;
            }
            set
            {
                this.GetOrCreatePropertySecurity(property).PropertyAccess = value;
            }
        }


        private BusinessObjectMemberSecurity GetOrCreatePropertySecurity(BusinessProperty property)
        {
            BusinessObjectMemberSecurity rs = this.Properties.SingleOrDefault(p => p.Id == property.Id);
            if (rs == null)
            {
                rs = new BusinessObjectMemberSecurity() { Id = property.Id, Name = property.Name };
                this.Properties.Add(rs);
            }

            return rs;

          
        }

        public static BusinessObjectSecurity Create(BusinessClass @class, BusinessObjectAccess? objectAccess, BusinessObjectAccess? propertyAccess)
        {
            BusinessObjectSecurity rs = new BusinessObjectSecurity();
            rs.ObjectAccess = objectAccess;
            rs.Sync(@class, propertyAccess);

            return rs;
        }

        private bool _syncing = false;

        public virtual void Sync(BusinessClass @class, BusinessObjectAccess? propertyAccess)
        {
            if (!_syncing)
            {
                _syncing = true;
                try
                {
                    SyncProperties(@class, propertyAccess);
                    SyncLeftAssn(@class, propertyAccess);
                    SyncRightAssn(@class, propertyAccess);
                }
                finally
                {
                    _syncing = false;
                }
               
            }
        }

        private void SyncLeftAssn(BusinessClass @class, BusinessObjectAccess? propertyAccess)
        {
            var toRemove = from ps in this.Properties
                           where ps.MemberType == BusinessObjectMemberTypes.LeftAssociation && !@class.AllLeftAssociations().Any(p => p.Id == ps.Id)
                           select ps;

            foreach (BusinessObjectMemberSecurity ps in toRemove.ToArray())
            {
                this.Properties.Remove(ps);
            }

            foreach (BusinessObjectMemberSecurity ps in this.Properties.Where(p=>p.MemberType== BusinessObjectMemberTypes.LeftAssociation))
            {
                BusinessAssociation assn = @class.AllLeftAssociations().First(p => p.Id == ps.Id);
                ps.Name = assn.RightRoleName;
                ps.DisplayOrder = assn.RightRoleDisplayOrder;
            }

            var toAdd = from assn in @class.AllLeftAssociations()
                        where !this.Properties.Any(p => p.Id == assn.Id && p.MemberType== BusinessObjectMemberTypes.LeftAssociation)
                        select
                            new BusinessObjectMemberSecurity() { Id = assn.Id, Name = assn.RightRoleName, PropertyAccess = propertyAccess, DisplayOrder = assn.RightRoleDisplayOrder, MemberType = BusinessObjectMemberTypes.LeftAssociation };

            foreach (BusinessObjectMemberSecurity ps in toAdd)
            {

                int index = this.Properties.BinarySearchBy(ps.DisplayOrder, (p => p.DisplayOrder));
                this.Properties.Insert(~index, ps);
            }
        }

        private void SyncRightAssn(BusinessClass @class, BusinessObjectAccess? propertyAccess)
        {
            var toRemove = from ps in this.Properties
                           where ps.MemberType == BusinessObjectMemberTypes.RightAssociation && !@class.AllRightAssociations().Any(p => p.Id == ps.Id)
                           select ps;

            foreach (BusinessObjectMemberSecurity ps in toRemove.ToArray())
            {
                this.Properties.Remove(ps);
            }

            foreach (BusinessObjectMemberSecurity ps in this.Properties.Where(p => p.MemberType == BusinessObjectMemberTypes.RightAssociation))
            {
                BusinessAssociation assn = @class.AllRightAssociations().First(p => p.Id == ps.Id);
                ps.Name = assn.LeftRoleName;
                ps.DisplayOrder = assn.RightRoleDisplayOrder;
            }

            var toAdd = from assn in @class.AllRightAssociations()
                        where !this.Properties.Any(p => p.Id == assn.Id && p.MemberType == BusinessObjectMemberTypes.RightAssociation)
                        select
                            new BusinessObjectMemberSecurity() { Id = assn.Id, Name = assn.LeftRoleName, PropertyAccess = propertyAccess, DisplayOrder = assn.LeftRoleDisplayOrder, MemberType = BusinessObjectMemberTypes.RightAssociation };

            foreach (BusinessObjectMemberSecurity ps in toAdd)
            {
                int index = this.Properties.BinarySearchBy(ps.DisplayOrder, (p => p.DisplayOrder));
                this.Properties.Insert(~index, ps);
            }
        }

        private void SyncProperties(BusinessClass @class, BusinessObjectAccess? propertyAccess)
        {
            var toRemove = from ps in this.Properties
                           where ps.MemberType == BusinessObjectMemberTypes.Property && !@class.AllProperties().Any(p => p.Id == ps.Id)
                           select ps;

            foreach (BusinessObjectMemberSecurity ps in toRemove.ToArray())
            {
                this.Properties.Remove(ps);
            }

            foreach (BusinessObjectMemberSecurity ps in this.Properties.Where(p=>p.MemberType == BusinessObjectMemberTypes.Property ))
            {
                BusinessProperty prop = @class.AllProperties().First(p => p.Id == ps.Id);
                ps.Name = prop.Name;
                ps.DisplayOrder = prop.DisplayOrder;
            }

            var toAdd = from prop in @class.AllProperties()
                        where !this.Properties.Any(p => p.Id == prop.Id)
                        select
                            new BusinessObjectMemberSecurity() { Id = prop.Id, Name = prop.Name, PropertyAccess = propertyAccess, DisplayOrder=prop.DisplayOrder, MemberType= BusinessObjectMemberTypes.Property };

            foreach (BusinessObjectMemberSecurity ps in toAdd)
            {
                int index = this.Properties.BinarySearchBy(ps.DisplayOrder, (p => p.DisplayOrder));
                this.Properties.Insert(~index, ps);
            }
        }

      

        protected virtual void OnChanged(EventArgs e)
        {
            
            if (!this._isLoading && this.Changed != null)
            {
                this.Changed(this, e);
            }
        }

        public event EventHandler<EventArgs> Changed;


        public BusinessObjectSecurity Union(BusinessObjectSecurity other)
        {
            BusinessObjectSecurity rs = new BusinessObjectSecurity();
            
            {
                BusinessObjectAccess? oa1 = this.ObjectAccess;
                BusinessObjectAccess? oa2 = other != null ? other.ObjectAccess : null;
                
                if (oa1 == null && oa2 == null)
                {
                    rs.ObjectAccess = null;
                }
                else if (oa1 == null)
                {
                    rs.ObjectAccess = oa2;
                }
                else if (oa2 == null)
                {
                    rs.ObjectAccess = oa1;
                }
                else
                {
                    rs.ObjectAccess = oa1 | oa2;
                }

            }

            foreach (BusinessObjectMemberSecurity ps1 in this.Properties)
            {
                BusinessObjectMemberSecurity ps2 = other.Properties.SingleOrDefault(p => p.Id == ps1.Id);
                BusinessObjectMemberSecurity nps = new BusinessObjectMemberSecurity() { Id = ps1.Id, Name = ps1.Name };

                BusinessObjectAccess? oa1 = ps1.PropertyAccess;
                BusinessObjectAccess? oa2 = ps2 != null ? ps2.PropertyAccess : null;

                if (oa1 == null && oa2 == null)
                {
                    nps.PropertyAccess = null;
                }
                else if (oa1 == null)
                {
                    nps.PropertyAccess = oa2;
                }
                else if (oa2 == null)
                {
                    nps.PropertyAccess = oa1;
                }
                else
                {
                    nps.PropertyAccess = oa1 | oa2;
                }

                rs.Properties.Add(nps);

            }
            return rs;
        }

        public BusinessObjectSecurity Intersect(BusinessObjectSecurity other)
        {
            BusinessObjectSecurity rs = new BusinessObjectSecurity();

            {
                BusinessObjectAccess? oa1 = this.ObjectAccess;
                BusinessObjectAccess? oa2 = other != null ? other.ObjectAccess : null;
                if (oa1 == null && oa2 == null)
                {
                    rs.ObjectAccess = null;
                }
                else if (oa2 == null)
                {
                    rs.ObjectAccess = oa1;
                }
                else if (oa1 == null)
                {
                    rs.ObjectAccess = oa2;
                }
                else
                {
                    rs.ObjectAccess = oa1 | oa2;
                }
            }


            foreach (BusinessObjectMemberSecurity ps1 in this.Properties)
            {
                BusinessObjectMemberSecurity ps2 = other.Properties.SingleOrDefault(p => p.Id == ps1.Id);
                BusinessObjectMemberSecurity nps = new BusinessObjectMemberSecurity() { Id = ps1.Id, Name = ps1.Name };

                BusinessObjectAccess? oa1 = ps1.PropertyAccess;
                BusinessObjectAccess? oa2 = ps2 != null ? ps2.PropertyAccess : null;

                if (oa1 == null && oa2 == null)
                {
                    nps.PropertyAccess = null;
                }
                else if (oa2 == null)
                {
                    nps.PropertyAccess = oa1;
                }
                else if (oa1 == null)
                {
                    nps.PropertyAccess = oa2;
                }
                else
                {
                    nps.PropertyAccess = oa1 | oa2;
                }
                rs.Properties.Add(nps);
            }

            return rs;
        }


        public static BusinessObjectSecurity operator | (BusinessObjectSecurity x, BusinessObjectSecurity y)
        {
            return x.Union(y);
        }

        public static BusinessObjectSecurity operator &(BusinessObjectSecurity x, BusinessObjectSecurity y)
        {
            return x.Intersect(y);
        }

        private bool _isLoading = false;
        public void Load(XElement element)
        {
            XSerializer ser = new XSerializer(typeof(BusinessObjectSecurity));
           
            this._isLoading = true;
            this.Properties.Clear();
            try
            {
                ser.Deserialize(element, this);
            }
            finally
            {
                this._isLoading = false;
            }

           
        }

        public XElement ToXElement()
        {
            XSerializer ser = new XSerializer(typeof(BusinessObjectSecurity));
            return ser.Serialize(this);
        }


    }
}
