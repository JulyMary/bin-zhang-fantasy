﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public class BusinessPackageCodeGenerator : ServiceBase, IBusinessPackageCodeGenerator
    {

        public override void InitializeService()
        {
            this._packageChangedListener = new WeakEventListener(PackageChanged);
            this._packageDeletedListener = new WeakEventListener(PackageDeleted);

            base.InitializeService();
        }


        #region IBusinessPackageCodeGenerator Members

        public void RegisterPackage(Fantasy.BusinessEngine.BusinessPackage package)
        {
            foreach (string propertyName in ListenedPackageProperties)
            {
                EntityPropertyChangedEventManager.AddListener(package, this._packageChangedListener, propertyName);
            }

            EntityStateChangedEventManager.AddListener(package, this._packageDeletedListener); 
        }

        #endregion

        private WeakEventListener _packageChangedListener;

        private WeakEventListener _packageDeletedListener;

        private static readonly string[] ListenedPackageProperties = new String[] { "CodeName", "ParentPackage" };

        private bool PackageChanged(Type managerType, object sender, EventArgs e)
        {

            BusinessPackage package = (BusinessPackage)sender;

            if (package.ParentPackage != null)
            {
                UpdateClassScripts(package);
                UpdateAppScripts(package);
                UpdateUserScripts(package);
                UpdateRoleScripts(package);
            }
            return true;
        }

        private void UpdateRoleScripts(BusinessPackage package)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            IBusinessRoleCodeGenerator gen = this.Site.GetRequiredService<IBusinessRoleCodeGenerator>();
            var roles = from childPackage in package.Flatten(p => p.ChildPackages)
                        from role in childPackage.Roles
                        where role.ScriptOptions == ScriptOptions.Default
                        select role;
            foreach (BusinessRoleData role in roles)
            {
                gen.Rename(role);
            }
        }

        private void UpdateUserScripts(BusinessPackage package)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            IBusinessUserCodeGenerator gen = this.Site.GetRequiredService<IBusinessUserCodeGenerator>();
            var users = from childPackage in package.Flatten(p => p.ChildPackages)
                       from user in childPackage.Users
                       where user.ScriptOptions == ScriptOptions.Default
                       select user;
            foreach (BusinessUserData user in users)
            {
                gen.Rename(user);
            }
        }

        private void UpdateAppScripts(BusinessPackage package)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            IApplicationCodeGenerator gen = this.Site.GetRequiredService<IApplicationCodeGenerator>();
            var apps = from childPackage in package.Flatten(p => p.ChildPackages)
                       from app in childPackage.Applications
                       where app.ScriptOptions == ScriptOptions.Default select app;
            foreach (BusinessApplicationData app in apps)
            {
                gen.Rename(app);
            }
        }

        private void UpdateClassScripts(BusinessPackage package)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();

            BusinessProperty[] clsProperties = (from cls in es.GetRootClass().Flatten(c => c.ChildClasses)
                                                from prop in cls.Properties
                                                where prop.DataType.Id == BusinessDataType.WellknownIds.Class
                                                select prop).ToArray();


            IBusinessClassCodeGenerator classGenerator = this.Site.GetRequiredService<IBusinessClassCodeGenerator>();

            BusinessClass[] descClasses = (from childPackage in package.Flatten(p => p.ChildPackages)
                                           from @class in childPackage.Classes
                                          
                                           select @class).ToArray();
            foreach (BusinessClass @class in descClasses)
            {
                if (@class.ScriptOptions == ScriptOptions.Default)
                {
                    classGenerator.Rename(@class);
                    classGenerator.UpdateAutoScript(@class);
                }
            }


            BusinessEnum[] enums = (from childPackage in package.Flatten(p => p.ChildPackages)
                                    from @enum in childPackage.Enums
                                    select @enum).ToArray();

            BusinessProperty[] enumProperties = (from cls in es.GetRootClass().Flatten(c => c.ChildClasses)
                                                 from prop in cls.Properties
                                                 where prop.DataType.Id == BusinessDataType.WellknownIds.Enum
                                                 select prop).ToArray();


            var relClasses = (from @class in descClasses
                              from childClass in @class.ChildClasses
                              select childClass)
                .Union(from @class in descClasses
                       from assn in @class.LeftAssociations
                       select assn.RightClass)
                .Union(from @class in descClasses
                       from assn in @class.RightAssociations
                       select assn.LeftClass)
                .Union(from @class in descClasses
                       from prop in clsProperties
                       where prop.DataClassType == @class
                       select prop.Class)
                .Union(from @enum in enums
                       from prop in enumProperties
                       where prop.DataEnumType == @enum
                       select prop.Class)
                .Distinct().Except(descClasses);
            foreach (BusinessClass @class in relClasses)
            {
                if (@class.ScriptOptions == ScriptOptions.Default)
                {
                    classGenerator.UpdateAutoScript(@class);
                }
            }
        }

        private bool PackageDeleted(Type managerType, object sender, EventArgs e)
        {
            BusinessPackage package = (BusinessPackage)sender;
            if (package.EntityState == EntityState.Deleted)
            {
                foreach (string propertyName in ListenedPackageProperties)
                {
                    EntityPropertyChangedEventManager.RemoveListener(package, this._packageChangedListener, propertyName);
                }

                EntityStateChangedEventManager.RemoveListener(package, this._packageDeletedListener); 
            }

            return true;
        }

    }
}
