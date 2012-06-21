using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.Windows;
using Fantasy.BusinessEngine.EntityExtensions;
using Fantasy.XSerialization;
using Fantasy.ComponentModel;
using Fantasy.BusinessEngine;
using Fantasy.Web.Properties;

namespace Fantasy.Web.EntityExtensions
{
    [XSerializable("BusinessObjectSelector", NamespaceUri = Consts.ExtensionsNamespace)]
    [ExtensionUsage(AllowMultiple = false)]
    [Icon(typeof(Resources), "BusinessObjectEditorIcon")]
    [ResourceCategory(typeof(WellknownCategoryNames), "Editor")]
    public class BusinessObjectSelectorExtension : NotifyPropertyChangedObject, IEntityExtension
    {

        [XAttribute("application")]
        private Guid _applicationId;

        public Guid ApplicationId
        {
            get { return _applicationId; }
            set
            {
                if (_applicationId != value)
                {
                    _applicationId = value;
                    this.OnPropertyChanged("ApplicationId");
                }
            }
        }

        private string _applicationName;

        public string ApplicationName
        {
            get { return _applicationName; }
            set
            {
                if (_applicationName != value)
                {
                    _applicationName = value;
                    this.OnPropertyChanged("ApplicationName");
                }
            }
        }



        [XAttribute("entryObject")]
        private Guid? _entryObjectId;

        public Guid? EntryObjectId
        {
            get { return _entryObjectId; }
            set
            {
                if (_entryObjectId != value)
                {
                    _entryObjectId = value;
                    this.OnPropertyChanged("EntryObjectId");
                }
            }
        }









        #region IEntityExtension Members

        public string Name
        {
            get { return Resources.ObjectEditorName; }
        }

        public string Description
        {
            get { return Resources.ObjectEditorDescriptor; }
        }

        public bool ApplyTo(object context)
        {
            return true;
        }

        #endregion
    }
}