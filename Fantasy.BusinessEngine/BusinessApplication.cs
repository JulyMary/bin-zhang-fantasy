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



        private ApplicationViewSettings _viewSettings;

        public ApplicationViewSettings ApplicationViewSettings
        {
            get
            {
                if (_viewSettings == null)
                {

                    if (!string.IsNullOrEmpty(this.Data.ViewSettings))
                    {
                        XSerializer ser = new XSerializer(typeof(ApplicationViewSettings));
                        XElement ele = XElement.Parse(this.Data.ViewSettings);
                        _viewSettings = (ApplicationViewSettings)ser.Deserialize(ele);
                    }
                    else
                    {
                        _viewSettings = new ApplicationViewSettings();
                    }

                }
                return _viewSettings;
            }
        }

        public virtual INavigationViewController GetNaviationView()
        {

            INavigationViewController rs; 

            if (ApplicationViewSettings.NavigationControllerType == null)
            {
                ApplicationViewSettings.NavigationControllerType = Type.GetType("Fantasy.Web.Controllers.StandardNavigationController, Fantasy.Web", true);
            }

            rs = (INavigationViewController)Activator.CreateInstance(ApplicationViewSettings.NavigationControllerType);

            if (rs is ICustomerizableViewController)
            {
                if (ApplicationViewSettings.NavigationViewSettings == null)
                {
                    Type settingsType = ApplicationViewSettings.NavigationControllerType.GetCustomAttribute<SettingsAttribute>(required: true).SettingsType;
                    ApplicationViewSettings.NavigationViewSettings = Activator.CreateInstance(settingsType);

                }

                ((ICustomerizableViewController)rs).LoadSettings(ApplicationViewSettings.NavigationViewSettings); 

            }

            return rs;
        }


        public virtual ViewType GetViewType(BusinessObject obj, string propertyName)
        {

            BusinessClass @class = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>().FindBusinessClass(obj.ClassId);

            

            //TODO: Use ObjectDescription to replace of following algorithm
            var lefts = from assn in @class.AllLeftAssociations()
                        where string.Equals(propertyName, assn.RightRoleCode, StringComparison.OrdinalIgnoreCase)
                        select
                            new { IsScalar = (new Cardinality(assn.RightCardinality)).IsSingleton, @class = assn.RightClass };
            var desc = lefts.SingleOrDefault();

            if (desc == null)
            {
                var rights = from assn in @class.AllRightAssociations()
                             where string.Equals(propertyName, assn.LeftRoleCode, StringComparison.OrdinalIgnoreCase)
                             select new { IsScalar = (new Cardinality(assn.LeftCardinality)).IsSingleton, @class=assn.LeftClass };

                desc = rights.Single(string.Format(Resources.PropertyNotFoundMessage, propertyName, @class.FullCodeName));
            }

            ClassViewSettings settings = this.GetClassViewSettings(desc.@class);
            if (settings != null)
            {
                return settings.ControllerType.IsAssignableFrom(typeof(IScalarViewController)) ? ViewType.Obj : ViewType.Col;
            }
            else
            {
                return desc.IsScalar || !desc.@class.IsSimple ? ViewType.Obj : ViewType.Col;
            }
           
        }


        

        public virtual IScalarViewController GetScalarView(BusinessObject obj)
        {
            BusinessClass @class = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>().FindBusinessClass(obj.Id);

            return (IScalarViewController)CreateDetialsViewController(@class, Type.GetType("Fantasy.Web.Controllers.StandardScalarController, Fantasy.Web", true));


        }

        private object CreateDetialsViewController(BusinessClass @class, Type defaultType)
        {
            ClassViewSettings viewSettings = GetClassViewSettings(@class);
            if (viewSettings == null)
            {
                viewSettings = new ClassViewSettings()
                {
                    ClassId = @class.Id,
                    ControllerType = defaultType
                };
                this.ApplicationViewSettings.ClassViewSettings.Add(viewSettings);
            }

            object rs = Activator.CreateInstance(viewSettings.ControllerType);

            if (rs is ICustomerizableViewController)
            {
                if (viewSettings.Settings == null)
                {
                    Type settingsType = ApplicationViewSettings.NavigationControllerType.GetCustomAttribute<SettingsAttribute>(required: true).SettingsType;
                    viewSettings.Settings = Activator.CreateInstance(settingsType);
                }

                ((ICustomerizableViewController)rs).LoadSettings(viewSettings.Settings);
            }

            return rs;
        }

        public virtual ICollectionViewController GetCollectionView(BusinessObject obj, string propertyName, IEnumerable<BusinessObject> collection)
        {
            BusinessClass parentClass = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>().FindBusinessClass(obj.ClassId);
            BusinessClass childClass;
            //TODO: Use ObjectDescription to replace of following algorithm
            var lefts = from assn in parentClass.AllLeftAssociations()
                        where string.Equals(propertyName, assn.RightRoleCode, StringComparison.OrdinalIgnoreCase)
                        select assn.RightClass;
            childClass = lefts.SingleOrDefault();




            if (childClass == null)
            {
                var rights = from assn in parentClass.AllRightAssociations()
                             where string.Equals(propertyName, assn.LeftRoleCode, StringComparison.OrdinalIgnoreCase)
                             select assn.RightClass;

                childClass = rights.Single(string.Format(Resources.PropertyNotFoundMessage, propertyName, parentClass.FullCodeName));
            }

            return (ICollectionViewController)CreateDetialsViewController(childClass, Type.GetType("Fantasy.Web.Controllers.StandardCollectionController, Fantasy.Web", true));

        }

        private ClassViewSettings GetClassViewSettings(BusinessClass @class)
        {
            ClassViewSettings viewSettings = this.ApplicationViewSettings.ClassViewSettings.SingleOrDefault(s => s.ClassId == @class.Id,
                string.Format(Resources.DuplicatedClassViewDefined, @class.FullName, this.Data.FullName));
            return viewSettings;
        }
    }
}
