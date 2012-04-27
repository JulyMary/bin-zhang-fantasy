using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Collections.ObjectModel;
using Fantasy.Windows;
using System.Xml.Linq;

namespace Fantasy.BusinessEngine.Security
{
    [XSerializable("object", NamespaceUri=Consts.SecurityNamespace)] 
    public class BusinessObjectSecurity : NotifyPropertyChangedObject
    {
        public BusinessObjectSecurity()
        {
            BusinessObjectMemberSecurityCollection properties = new BusinessObjectMemberSecurityCollection();
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

        [XAttribute("canCreate")]
        private bool? _canCreate;

        public bool? CanCreate
        {
            get { return _canCreate; }
            set
            {
                if (_canCreate != value)
                {
                    _canCreate = value;
                    this.OnPropertyChanged("CanCreate");
                }
            }
        }

        [XAttribute("canDelete")]
        private bool? _canDelete;

        public bool? CanDelete
        {
            get { return _canDelete; }
            set
            {
                if (_canDelete != value)
                {
                    _canDelete = value;
                    this.OnPropertyChanged("CanDelete");
                }
            }
        }

        [XArray,
        XArrayItem(Name = "property", Type=typeof(BusinessObjectMemberSecurity))]
        public virtual BusinessObjectMemberSecurityCollection Properties { get; protected set; }

        private BusinessObjectMemberSecurity GetOrCreatePropertySecurity(BusinessProperty property)
        {
            BusinessObjectMemberSecurity rs = this.Properties.SingleOrDefault(p => p.Id == property.Id);
            if (rs == null)
            {
                rs = new BusinessObjectMemberSecurity() { Id = property.Id, Name = property.CodeName };
                this.Properties.Add(rs);
            }

            return rs;

          
        }

        public static BusinessObjectSecurity Create(BusinessClass @class, bool? canCreate, bool? canDelete, bool? canRead, bool? canWrite)
        {
            BusinessObjectSecurity rs = new BusinessObjectSecurity() { _canCreate = canCreate, _canDelete = canDelete };
            
            rs.Sync(@class, canRead, canWrite);

            return rs;
        }

        private bool _syncing = false;

        public virtual void Sync(BusinessClass @class, bool? canRead, bool? canWrite)
        {
            if (!_syncing)
            {
                _syncing = true;
                try
                {
                    SyncProperties(@class, canRead, canWrite);
                    SyncLeftAssn(@class, canRead, canWrite);
                    SyncRightAssn(@class, canRead, canWrite);
                }
                finally
                {
                    _syncing = false;
                }
               
            }
        }

        public static bool? NullableAnd(bool? x, bool? y)
        {
            if (x == null && y == null)
            {
                return null;
            }
            else if (x == null)
            {
                return y;
            }
            else if (y == null)
            {
                return x;
            }
            else
            {
                return (bool)x && (bool)y;
            }

        }

        public static bool? NullableOr(bool? x, bool? y)
        {
            if (x == null && y == null)
            {
                return null;
            }
            else if (x == null)
            {
                return y;
            }
            else if (y == null)
            {
                return x;
            }
            else
            {
                return (bool)x || (bool)y;
            }
        }


        private void SyncLeftAssn(BusinessClass @class, bool? canRead, bool? canWrite)
        {
            var toRemove = from ps in this.Properties
                           where ps.MemberType == BusinessObjectMemberTypes.LeftAssociation && !@class.AllLeftAssociations().Any(assn => assn.Id == ps.Id && assn.RightNavigatable == true)
                           select ps;

            foreach (BusinessObjectMemberSecurity ps in toRemove.ToArray())
            {
                this.Properties.Remove(ps);
            }

            foreach (BusinessObjectMemberSecurity ps in this.Properties.Where(p=>p.MemberType== BusinessObjectMemberTypes.LeftAssociation))
            {
                BusinessAssociation assn = @class.AllLeftAssociations().First(p => p.Id == ps.Id);
                ps.Name = assn.RightRoleCode;
                ps.DisplayOrder = assn.RightRoleDisplayOrder;
            }

            var toAdd = from assn in @class.AllLeftAssociations()
                        where assn.RightNavigatable == true &&  !this.Properties.Any(p => p.Id == assn.Id && p.MemberType== BusinessObjectMemberTypes.LeftAssociation)
                        select
                            new BusinessObjectMemberSecurity() { Id = assn.Id, Name = assn.RightRoleCode, CanRead=canRead, CanWrite = canWrite, DisplayOrder = assn.RightRoleDisplayOrder, MemberType = BusinessObjectMemberTypes.LeftAssociation };

            foreach (BusinessObjectMemberSecurity ps in toAdd)
            {

                int index = this.Properties.BinarySearchBy(ps.DisplayOrder, (p => p.DisplayOrder));
                this.Properties.Insert(~index, ps);
            }
        }

        private void SyncRightAssn(BusinessClass @class, bool? canRead, bool? canWrite)
        {
            var toRemove = from ps in this.Properties
                           where ps.MemberType == BusinessObjectMemberTypes.RightAssociation && !@class.AllRightAssociations().Any(assn => assn.Id == ps.Id && assn.LeftNavigatable == true)
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
                        where assn.LeftNavigatable == true && !this.Properties.Any(p => p.Id == assn.Id && p.MemberType == BusinessObjectMemberTypes.RightAssociation)
                        select
                            new BusinessObjectMemberSecurity() { Id = assn.Id, Name = assn.LeftRoleCode, CanRead = canRead, CanWrite = canWrite, DisplayOrder = assn.LeftRoleDisplayOrder, MemberType = BusinessObjectMemberTypes.RightAssociation };

            foreach (BusinessObjectMemberSecurity ps in toAdd)
            {
                int index = this.Properties.BinarySearchBy(ps.DisplayOrder, (p => p.DisplayOrder));
                this.Properties.Insert(~index, ps);
            }
        }

        private void SyncProperties(BusinessClass @class, bool? canRead, bool? canWrite)
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
                ps.Name = prop.CodeName;
                ps.DisplayOrder = prop.DisplayOrder;
            }

            var toAdd = from prop in @class.AllProperties()
                        where !this.Properties.Any(p => p.Id == prop.Id)
                        select
                            new BusinessObjectMemberSecurity() { Id = prop.Id, Name = prop.CodeName, CanRead=canRead, CanWrite=canWrite, DisplayOrder=prop.DisplayOrder, MemberType= BusinessObjectMemberTypes.Property };

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

            rs.CanCreate = NullableOr(this.CanCreate, other != null ? other.CanCreate : null);
            rs.CanDelete = NullableOr(this.CanDelete, other != null ? other.CanDelete : null);

            foreach (BusinessObjectMemberSecurity ps1 in this.Properties)
            {
                BusinessObjectMemberSecurity ps2 = other.Properties.SingleOrDefault(p => p.Id == ps1.Id && p.MemberType == ps1.MemberType);
                BusinessObjectMemberSecurity nps = new BusinessObjectMemberSecurity() { Id = ps1.Id, Name = ps1.Name, DisplayOrder=ps1.DisplayOrder, MemberType = ps1.MemberType };

                nps.CanRead = NullableOr(ps1.CanRead, ps2 != null ? ps2.CanRead : null);
                nps.CanWrite = NullableOr(ps1.CanWrite, ps2 != null ? ps2.CanWrite : null);
                

                rs.Properties.Add(nps);

            }
            return rs;
        }

        public BusinessObjectSecurity Intersect(BusinessObjectSecurity other)
        {
            BusinessObjectSecurity rs = new BusinessObjectSecurity();

            rs.CanCreate = NullableAnd(this.CanCreate, other != null ? other.CanCreate : null);
            rs.CanDelete = NullableAnd(this.CanDelete, other != null ? other.CanDelete : null);

            foreach (BusinessObjectMemberSecurity ps1 in this.Properties)
            {
                BusinessObjectMemberSecurity ps2 = other.Properties.SingleOrDefault(p => p.Id == ps1.Id && p.MemberType == ps1.MemberType);
                BusinessObjectMemberSecurity nps = new BusinessObjectMemberSecurity() { Id = ps1.Id, Name = ps1.Name, DisplayOrder = ps1.DisplayOrder, MemberType = ps1.MemberType };

                nps.CanRead = NullableAnd(ps1.CanRead, ps2 != null ? ps2.CanRead : null);
                nps.CanWrite = NullableAnd(ps1.CanWrite, ps2 != null ? ps2.CanWrite : null);
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
