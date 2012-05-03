using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Fantasy.BusinessEngine;
using Fantasy.XSerialization;

namespace Fantasy.Web
{
    [XSerializable("viewSettings", NamespaceUri=Consts.ViewSettingsNamespace)]
    public class ApplicationViewSettings : IXSerializable
    {
        public ApplicationViewSettings()
        {
            ClassViewSettings = new List<ClassViewSettings>();
        }

        private static XNamespace ns = Consts.ViewSettingsNamespace;

        public Type NavigationControllerType { get; set; }

        public object NavigationViewSettings {get;set;}

      
        [XArray,
        XArrayItem(Type=typeof(ClassViewSettings), Name="class")]
        public IList<ClassViewSettings> ClassViewSettings { get; private set; }

       

        #region IXSerializable Members

        public void Load(IServiceProvider context, XElement element)
        {

            XHelper.Default.LoadByXAttributes(context, element, this);

            XElement nav = element.Element(ns + "navigation");
            if (nav != null)
            {
                this.NavigationControllerType = Type.GetType((string)nav.Attribute(ns + "type"), true);
                if (this.NavigationControllerType.IsAssignableFrom(typeof(ICustomerizableViewController)))
                {
                    XSerializer ser = new XSerializer(NavigationControllerType.GetCustomAttribute<SettingsAttribute>().SettingsType);
                    this.NavigationViewSettings = ser.Deserialize(nav);
                }
            }
        }

        public void Save(IServiceProvider context, XElement element)
        {

            XHelper.Default.SaveByXAttributes(context, element, this);
            if (this.NavigationViewSettings != null)
            {
                XElement nav = new XElement(ns + "navigation");
                nav.SetAttributeValue(ns + "type", NavigationControllerType.VersionFreeTypeName());
                if (this.NavigationViewSettings != null && this.NavigationControllerType.IsAssignableFrom(typeof(ICustomerizableViewController)))
                {
                    XSerializer ser = new XSerializer(NavigationControllerType.GetCustomAttribute<SettingsAttribute>().SettingsType);
                    ser.Serialize(nav, this.NavigationViewSettings);
                    element.Add(nav);
                }
            }
        }

        #endregion
    }


}
