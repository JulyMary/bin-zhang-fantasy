﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using Fantasy.Windows;

namespace Fantasy.BusinessEngine
{
    [XSerializable("property", NamespaceUri=Consts.SecurityNamespace)]
    public class BusinessObjectMemberSecurity : NotifyPropertyChangedObject
    {
        [XAttribute("id")]
        private Guid _id;

        public Guid Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    this.OnPropertyChanged("Id");
                }
            }
        }

        [XAttribute("name")]
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        [XAttribute("access")]
        private BusinessObjectAccess? _propertyAccess;

        public BusinessObjectAccess? PropertyAccess
        {
            get { return _propertyAccess; }
            set
            {
                if (_propertyAccess != value)
                {
                    _propertyAccess = value;
                    this.OnPropertyChanged("PropertyAccess");
                }
            }
        }

        [XAttribute("type")]
        private BusinessObjectMemberTypes _memberType;

        public BusinessObjectMemberTypes MemberType
        {
            get { return _memberType; }
            set
            {
                if (_memberType != value)
                {
                    _memberType = value;
                    this.OnPropertyChanged("MemberType");
                }
            }
        }

        [XAttribute("order")]
        private long _displayOrder;

        public long DisplayOrder
        {
            get { return _displayOrder; }
            set
            {
                if (_displayOrder != value)
                {
                    _displayOrder = value;
                    this.OnPropertyChanged("DisplayOrder");
                }
            }
        }


    }
}