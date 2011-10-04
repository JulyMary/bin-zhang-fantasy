using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public class BusinessEnumCodeGenerator : ServiceBase, Fantasy.Studio.BusinessEngine.CodeGenerating.IBusinessEnumCodeGenerator
    {
        public override void InitializeService()
        {
            this._enumChangedListener = new WeakEventListener(EnumChanged);
            this._enumDeletedListener = new WeakEventListener(EnumDeleted);

            base.InitializeService();
        }


        #region IBusinessPackageCodeGenerator Members

        public void RegisterEnum(Fantasy.BusinessEngine.BusinessEnum @enum)
        {
            foreach (string propertyName in ListenedPackageProperties)
            {
                EntityPropertyChangedEventManager.AddListener(@enum, this._enumChangedListener, propertyName);
            }

            EntityStateChangedEventManager.AddListener(@enum, this._enumDeletedListener);
        }

        #endregion

        private WeakEventListener _enumChangedListener;

        private WeakEventListener _enumDeletedListener;

        private static readonly string[] ListenedPackageProperties = new String[] { "CodeName", "ParentPackage" };

        private bool EnumChanged(Type managerType, object sender, EventArgs e)
        {

            BusinessEnum @enum = (BusinessEnum)sender;

            if (@enum.Package != null)
            {
                IEntityService es = this.Site.GetRequiredService<IEntityService>();
                IBusinessClassCodeGenerator classGenerator = this.Site.GetRequiredService<IBusinessClassCodeGenerator>();
                BusinessClass[] relClasses = (from cls in es.GetRootClass().Flatten(c => c.ChildClasses)
                                              from prop in cls.Properties
                                              where prop.DataType.Id == BusinessDataType.WellknownIds.Enum
                                              && prop.DataEnumType == @enum
                                              select prop.Class).Distinct().ToArray();

               
               
                foreach (BusinessClass @class in relClasses)
                {
                    classGenerator.UpdateAutoScript(@class);
                }
            }
            return true;
        }

        private bool EnumDeleted(Type managerType, object sender, EventArgs e)
        {
            BusinessEnum @enum = (BusinessEnum)sender;
            if (@enum.EntityState == EntityState.Deleted)
            {
                foreach (string propertyName in ListenedPackageProperties)
                {
                    EntityPropertyChangedEventManager.RemoveListener(@enum, this._enumChangedListener, propertyName);
                }

                EntityStateChangedEventManager.RemoveListener(@enum, this._enumDeletedListener);
            }

            return true;
        }
    }
}
