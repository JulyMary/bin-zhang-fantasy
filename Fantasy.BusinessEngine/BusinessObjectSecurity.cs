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
            ObservableCollection<BusinessObjectPropertySecurity> properties = new ObservableCollection<BusinessObjectPropertySecurity>();
            properties.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PropertiesChanged);
            this.Properties = properties;

        }

        void PropertiesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.IsDirty = true;

            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                if (e.NewItems != null)
                {
                    foreach (BusinessObjectPropertySecurity ps in e.NewItems)
                    {
                        ps.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(PropertySecurityChanged);
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (BusinessObjectPropertySecurity ps in e.OldItems)
                    {
                        ps.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(PropertySecurityChanged);
                    }
                }
            }

        }

        void PropertySecurityChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.IsDirty = true;
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
                    this.IsDirty = true;
                }
            }
        }

        [XArray,
        XArrayItem(Name = "property", Type=typeof(BusinessObjectPropertySecurity))]
        public virtual IList<BusinessObjectPropertySecurity> Properties { get; protected set; }


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


        private BusinessObjectPropertySecurity GetOrCreatePropertySecurity(BusinessProperty property)
        {
            BusinessObjectPropertySecurity rs = this.Properties.SingleOrDefault(p => p.Id == property.Id);
            if (rs == null)
            {
                rs = new BusinessObjectPropertySecurity() { Id = property.Id, Name = property.Name };
                this.Properties.Add(rs);
            }

            return rs;

          
        }

        public static BusinessObjectSecurity Create(BusinessClass @class, BusinessObjectAccess? objectAccess, BusinessObjectAccess? propertyAccess)
        {
            BusinessObjectSecurity rs = new BusinessObjectSecurity();
            rs.ObjectAccess = objectAccess;
            foreach (BusinessProperty property in @class.AllProperties())
            {
                BusinessObjectPropertySecurity ps = new BusinessObjectPropertySecurity() { Id = property.Id, Name = property.Name, PropertyAccess=propertyAccess };
                rs.Properties.Add(ps);
            }

            return rs;
        }


        public virtual void Sync(BusinessClass @class, BusinessObjectAccess? propertyAccess)
        {
            var toRemove = from ps in this.Properties
                        where !@class.AllProperties().Any(p => p.Id == ps.Id)
                        select ps;

            foreach (BusinessObjectPropertySecurity ps in toRemove.ToArray())
            {
                this.Properties.Remove(ps);
            }

            foreach (BusinessObjectPropertySecurity ps in this.Properties)
            {
                BusinessProperty prop = @class.AllProperties().First(p=>p.Id == ps.Id);
                ps.Name = prop.Name; 
            }

            var toAdd = from prop in @class.AllProperties() where !this.Properties.Any(p => p.Id == prop.Id) select
                       new BusinessObjectPropertySecurity() { Id = prop.Id, Name = prop.Name, PropertyAccess = propertyAccess };

            foreach (BusinessObjectPropertySecurity ps in toAdd)
            {
                this.Properties.Add(ps);
            }

        }

        private bool _isDirty;
        private bool _isLoading = false;

        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    if (!this._isLoading)
                    {
                        this.OnPropertyChanged("IsDirty");
                    };
                }
            }
        }

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

            foreach (BusinessObjectPropertySecurity ps1 in this.Properties)
            {
                BusinessObjectPropertySecurity ps2 = other.Properties.SingleOrDefault(p => p.Id == ps1.Id);
                BusinessObjectPropertySecurity nps = new BusinessObjectPropertySecurity() { Id = ps1.Id, Name = ps1.Name };

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


            foreach (BusinessObjectPropertySecurity ps1 in this.Properties)
            {
                BusinessObjectPropertySecurity ps2 = other.Properties.SingleOrDefault(p => p.Id == ps1.Id);
                BusinessObjectPropertySecurity nps = new BusinessObjectPropertySecurity() { Id = ps1.Id, Name = ps1.Name };

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

            this.IsDirty = false;
        }

        public XElement ToXElement()
        {
            XSerializer ser = new XSerializer(typeof(BusinessObjectSecurity));
            return ser.Serialize(this);
        }


    }
}
