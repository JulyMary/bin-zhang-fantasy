using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine.Security;
using Fantasy.Web;
using Fantasy.XSerialization;
using System.Xml.Linq;
using Fantasy.BusinessEngine.Properties;


namespace Fantasy.BusinessEngine
{
    public class BusinessApplication 
    {
       
        public BusinessApplicationData Data { get; internal set; }

        private BusinessObject _entryObject = null;

        public virtual BusinessObject EntryObject
        {
            get 
            {
                if (this._entryObject == null && this.Data.EntryObjectId != null)
                {
                    this._entryObject = BusinessEngineContext.Current.GetRequiredService<IEntityService>().Get<BusinessObject>(this.Data.EntryObjectId);
                }
                return _entryObject; 
            }
            set 
            {
                _entryObject = value;
                
            }
        }


        public virtual void Load()
        {

        }


        public virtual void Unload()
        {

        }


        public virtual BusinessObjectSecurity GetObjectSecurity(BusinessObject obj)
        {

            BusinessUser user = BusinessEngineContext.Current.User;
            ObjectSecurityArgs args = new ObjectSecurityArgs() { Application = this, Object = obj, User = user };

            return BusinessEngineContext.Current.GetRequiredService<ISecurityService>().GetObjectSecurity(args);
        }


        public virtual BusinessObjectSecurity GetClassSecurity(BusinessClass @class)
        {
            BusinessUser user = BusinessEngineContext.Current.User;
            ClassSecurityArgs args = new ClassSecurityArgs() { Application = this, Class=@class, User = user };
            return BusinessEngineContext.Current.GetRequiredService<ISecurityService>().GetClassSecurity(args);
        }



        private XElement _viewSettings;

        public XElement ApplicationViewSettings
        {
            get
            {
                if (_viewSettings == null)
                {

                    if (!string.IsNullOrEmpty(this.Data.ViewSettings))
                    {

                        _viewSettings = XElement.Parse(this.Data.ViewSettings);
                    }
                    else
                    {
                        XNamespace ns = Consts.ViewSettingsNamespace;
                        _viewSettings = new XElement(ns + "settings"); 
                    }

                }
                return _viewSettings;
            }
        }

        public virtual INavigationViewController GetNaviationView()
        {
            XNamespace ns = Consts.ViewSettingsNamespace;

            INavigationViewController rs;

            XElement settings = ApplicationViewSettings.Element(ns + "navigation");
            
            if (settings == null)
            {
                settings = new XElement(ns + "navigation", new XAttribute(ns + "controller", "Fantasy.Web.Controllers.StandardNavigationController, Fantasy.Web"));
                ApplicationViewSettings.Add(settings);
            }

            Type controllerType = Type.GetType((string)settings.Attribute(ns + "controller"), true);




            rs = (INavigationViewController)Activator.CreateInstance(controllerType);

            if (rs is ICustomerizableViewController)
            {
               

                ((ICustomerizableViewController)rs).LoadSettings(settings); 

            }

            return rs;
        }


        public virtual ViewType GetViewType(BusinessObject obj, string propertyName)
        {

            BusinessClass @class = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>().FindBusinessClass(obj.ClassId);


            BusinessObjectDescriptor objDesc = new BusinessObjectDescriptor(obj);


            BusinessPropertyDescriptor propDesc = objDesc.Properties[propertyName];
            

            XElement settings = this.GetClassViewSettings(propDesc.ReferencedClass);

            if (settings != null)
            {   XNamespace ns = Consts.ViewSettingsNamespace;
                Type controllerType = Type.GetType((string)settings.Attribute(ns + "controller"), true);
                return typeof(IScalarViewController).IsAssignableFrom(controllerType) ? ViewType.Obj : ViewType.Col;
            }
            else
            {
                return propDesc.IsScalar || !propDesc.ReferencedClass.IsSimple ? ViewType.Obj : ViewType.Col;
            }
           
        }


        public virtual IScalarViewController GetScalarView(BusinessClass @class)
        {
            return (IScalarViewController)CreateDetialsViewController(@class, Type.GetType("Fantasy.Web.Controllers.StandardScalarController, Fantasy.Web", true));
        }

        public virtual IScalarViewController GetScalarView(BusinessObject obj)
        {
            BusinessClass @class = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>().FindBusinessClass(obj.ClassId);
            return GetScalarView(@class);
        }

        private object CreateDetialsViewController(BusinessClass @class, Type defaultType)
        {
            XNamespace ns = Consts.ViewSettingsNamespace;
            XElement viewSettings = GetClassViewSettings(@class);
            Type controllerType = defaultType;
            if (viewSettings == null)
            {
                viewSettings = new XElement(ns + "detail", new XAttribute(ns + "classId", @class.Id), new XAttribute(ns + "controller", defaultType.VersionFreeTypeName()));

                this.ApplicationViewSettings.Add(viewSettings);
            }
            else
            {
                controllerType = Type.GetType((string)viewSettings.Attribute(ns + "controller"), true);
            }

            object rs = Activator.CreateInstance(controllerType);

            if (rs is ICustomerizableViewController)
            {
                ((ICustomerizableViewController)rs).LoadSettings(viewSettings);
            }

            return rs;
        }

        public virtual ICollectionViewController GetCollectionView(BusinessObject obj, string propertyName)
        {
            BusinessClass parentClass = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>().FindBusinessClass(obj.ClassId);

            BusinessObjectDescriptor objDesc = new BusinessObjectDescriptor(obj);


            BusinessPropertyDescriptor propDesc = objDesc.Properties[propertyName];
        

            return (ICollectionViewController)CreateDetialsViewController(propDesc.ReferencedClass, Type.GetType("Fantasy.Web.Controllers.StandardCollectionController, Fantasy.Web", true));

        }

        private XElement GetClassViewSettings(BusinessClass @class)
        {
            XNamespace ns = Consts.ViewSettingsNamespace;
            XElement rs = this.ApplicationViewSettings.Elements(ns + "detail").FirstOrDefault(e => (Guid)e.Attribute(ns + "classId") == @class.Id);
           
            return rs;
        }
    }
}
