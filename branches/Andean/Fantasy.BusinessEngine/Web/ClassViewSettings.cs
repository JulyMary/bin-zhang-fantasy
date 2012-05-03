using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Xml.Linq;
using Fantasy.BusinessEngine;


namespace Fantasy.Web
{
    [XSerializable("class", NamespaceUri=Consts.ViewSettingsNamespace)]
    public class ClassViewSettings : IXSerializable
    {
        private static XNamespace ns = Consts.ViewSettingsNamespace;
        public Guid ClassId { get; set; }

        public Type ControllerType { get; set; }

        public object Settings { get; set; }

        private Type GetSettingsType()
        {
            return ControllerType.GetCustomAttribute<SettingsAttribute>().SettingsType; 
        }

        #region IXSerializable Members

        public void Load(IServiceProvider context, System.Xml.Linq.XElement element)
        {
            this.ClassId = (Guid)element.Attribute(ns + "id");
            this.ControllerType = Type.GetType((string)element.Attribute(ns + "type"), true);
            if (this.ControllerType.IsAssignableFrom(typeof(ICustomerizableViewController)))
            {
                XSerializer ser = new XSerializer(GetSettingsType());
                this.Settings = ser.Deserialize(element);
            }
        }

        public void Save(IServiceProvider context, System.Xml.Linq.XElement element)
        {

            element.SetAttributeValue(ns + "id", this.ClassId);
            element.SetAttributeValue(ns + "type", this.ControllerType.VersionFreeTypeName());
            if (this.Settings != null && this.ControllerType.IsAssignableFrom(typeof(ICustomerizableViewController)))
            {
                XSerializer ser = new XSerializer(GetSettingsType());
                ser.Serialize(element, this.Settings);
               
            }
        }

        #endregion
    }
}
