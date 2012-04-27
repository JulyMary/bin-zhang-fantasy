using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using Fantasy.Windows;

namespace Fantasy.BusinessEngine.Security
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

        [XAttribute("canRead")]
        private bool? _canRead;

        public bool? CanRead
        {
            get { return _canRead; }
            set
            {
                if (_canRead != value)
                {
                    _canRead = value;
                    this.OnPropertyChanged("CanRead");
                }
            }
        }

        private bool? _canWrite;

        public bool? CanWrite
        {
            get { return _canWrite; }
            set
            {
                if (_canWrite != value)
                {
                    _canWrite = value;
                    this.OnPropertyChanged("CanWrite");
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


        public override string ToString()
        {
            return string.Format("{0} CanRead: {1}, CanWrite {2}", this.Name, this.CanRead, this.CanWrite); 
        }

    }
}
