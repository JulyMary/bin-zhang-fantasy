using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using Fantasy.Studio.BusinessEngine.Properties;
using Microsoft.VisualStudio.TextTemplating;
using Fantasy.IO;
using System.ComponentModel;
using System.Collections.Specialized;
using Fantasy.BusinessEngine.Collections;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Fantasy.BusinessEngine.Events;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public class BusinessClassCodeGenerator : ServiceBase, IBusinessClassCodeGenerator
    {
        private Dictionary<object, BusinessClass> _listenedCollections = new Dictionary<object, BusinessClass>();

        public override void InitializeService()
        {

            this._classChangedListener = new WeakEventListener(this.ClassChanged);
            this._classDeletedListener = new WeakEventListener(this.ClassDeleted);
            this._propertyCollectionChangedListener = new WeakEventListener(this.PropertyCollectionChanged);

            this._leftAssnChangedListener = new WeakEventListener(this.LeftAssnChanged);
            this._leftAssnCollectionChangedListener = new WeakEventListener(this.LeftAssnCollectionChanged);
            this._rightAssnChangedListener = new WeakEventListener(this.RightAssnChanged);
            this._rightAssnCollectionChangedListener = new WeakEventListener(this.RightAssnCollectionChanged);
            base.InitializeService();
        }

        public void RegisterClass(Fantasy.BusinessEngine.BusinessClass @class)
        {
            foreach (string propertyName in ListenedClassProperties)
            {
                EntityPropertyChangedEventManager.AddListener(@class, this._classChangedListener, propertyName);
            }


            CollectionChangedEventManager.AddListener(@class.Properties, this._propertyCollectionChangedListener);
            this._listenedCollections.Add(@class.Properties, @class);


            foreach (BusinessProperty property in @class.Properties)
            {
                foreach (string propertyName in ListenedPropertyProperties)
                {
                    EntityPropertyChangedEventManager.AddListener(property, this._classChangedListener, propertyName);
                }
            }


            CollectionChangedEventManager.AddListener(@class.LeftAssociations, this._leftAssnCollectionChangedListener);
            this._listenedCollections.Add(@class.LeftAssociations, @class);

            foreach (BusinessAssociation assn in @class.LeftAssociations)
            {
                foreach (string propertyName in ListenedLeftAssnProperties)
                {
                    EntityPropertyChangedEventManager.AddListener(assn, this._leftAssnChangedListener, propertyName);
                }
            }


            CollectionChangedEventManager.AddListener(@class.RightAssociations, this._rightAssnCollectionChangedListener);
            this._listenedCollections.Add(@class.RightAssociations, @class);
            foreach (BusinessAssociation assn in @class.RightAssociations)
            {
                foreach (string propertyName in ListenedRightAssnProperties)
                {
                    EntityPropertyChangedEventManager.AddListener(assn, this._rightAssnChangedListener, propertyName);
                }
            }


            EntityStateChangedEventManager.AddListener(@class, _classDeletedListener);
        }

        private static readonly string[] ListenedClassProperties = new string[] { "CodeName", "Package", "ParentClass" };
        private static readonly string[] ListenedPropertyProperties = new string[] { "CodeName", "DataType", "DataClassType", "DataEnumType", "IsNullable", "IsCalculated" };
        private static readonly string[] ListenedLeftAssnProperties = new string[] { "RightRoleCode", "RightCardinality", "RightNavigatable" };
        private static readonly string[] ListenedRightAssnProperties = new string[] { "LeftRoleCode", "LeftCardinality", "LeftNavigatable" };



        private WeakEventListener _classChangedListener;
        private WeakEventListener _classDeletedListener;
        private WeakEventListener _propertyCollectionChangedListener;
        private WeakEventListener _leftAssnChangedListener;
        private WeakEventListener _rightAssnChangedListener;
        private WeakEventListener _leftAssnCollectionChangedListener;
        private WeakEventListener _rightAssnCollectionChangedListener;


        private bool LeftAssnChanged(Type managerType, object sender, EventArgs e)
        {
            BusinessAssociation assn = (BusinessAssociation)sender;
            this.UpdateAutoScript(assn.LeftClass);
            return true;
        }

        private bool RightAssnChanged(Type managerType, object sender, EventArgs e)
        {
            BusinessAssociation assn = (BusinessAssociation)sender;
            this.UpdateAutoScript(assn.RightClass);
            return true;
        }

        private bool LeftAssnCollectionChanged(Type managerType, object sender, EventArgs e)
        {
            NotifyCollectionChangedEventArgs args = (NotifyCollectionChangedEventArgs)e;
            if (args.Action != NotifyCollectionChangedAction.Move)
            {
                if (args.OldItems != null)
                {
                    foreach (BusinessAssociation assn in args.OldItems)
                    {
                        foreach (string propertyName in ListenedLeftAssnProperties)
                        {
                            EntityPropertyChangedEventManager.RemoveListener(assn, this._leftAssnChangedListener, propertyName);
                        }
                    }
                }
                if (args.NewItems != null)
                {
                    foreach (BusinessAssociation assn in args.NewItems)
                    {
                        foreach (string propertyName in ListenedLeftAssnProperties)
                        {
                            EntityPropertyChangedEventManager.AddListener(assn, this._leftAssnChangedListener, propertyName);
                        }
                    }
                }
            }


            BusinessClass @class = this._listenedCollections[sender];
            UpdateAutoScript(@class);
            return true;
        }


        private bool RightAssnCollectionChanged(Type managerType, object sender, EventArgs e)
        {
            NotifyCollectionChangedEventArgs args = (NotifyCollectionChangedEventArgs)e;
            if (args.Action != NotifyCollectionChangedAction.Move)
            {
                if (args.OldItems != null)
                {
                    foreach (BusinessAssociation assn in args.OldItems)
                    {
                        foreach (string propertyName in ListenedRightAssnProperties)
                        {
                            EntityPropertyChangedEventManager.RemoveListener(assn, this._rightAssnChangedListener, propertyName);
                        }
                    }
                }
                if (args.NewItems != null)
                {
                    foreach (BusinessAssociation assn in args.NewItems)
                    {
                        foreach (string propertyName in ListenedRightAssnProperties)
                        {
                            EntityPropertyChangedEventManager.AddListener(assn, this._rightAssnChangedListener, propertyName);
                        }
                    }
                }
            }


            BusinessClass @class = this._listenedCollections[sender];
            UpdateAutoScript(@class);
            return true;
        }

        private bool ClassDeleted(Type managerType, object sender, EventArgs e)
        {
            BusinessClass @class = (BusinessClass)sender;
            if (@class.EntityState == EntityState.Deleted)
            {
                foreach (string propertyName in ListenedClassProperties)
                {
                    EntityPropertyChangedEventManager.RemoveListener(@class, this._classChangedListener, propertyName);
                }

                CollectionChangedEventManager.RemoveListener(@class.Properties, this._propertyCollectionChangedListener);
                this._listenedCollections.Remove(@class.Properties);

                foreach (BusinessProperty property in @class.Properties)
                {
                    foreach (string propertyName in ListenedPropertyProperties)
                    {
                        EntityPropertyChangedEventManager.RemoveListener(property, this._classChangedListener, propertyName);
                    }
                }


                CollectionChangedEventManager.RemoveListener(@class.LeftAssociations, this._leftAssnCollectionChangedListener);
                this._listenedCollections.Remove(@class.LeftAssociations);

                foreach (BusinessAssociation assn in @class.LeftAssociations)
                {
                    foreach (string propertyName in ListenedLeftAssnProperties)
                    {
                        EntityPropertyChangedEventManager.RemoveListener(assn, this._leftAssnChangedListener, propertyName);
                    }
                }


                CollectionChangedEventManager.AddListener(@class.RightAssociations, this._rightAssnCollectionChangedListener);
                this._listenedCollections.Remove(@class.RightAssociations);
                foreach (BusinessAssociation assn in @class.RightAssociations)
                {
                    foreach (string propertyName in ListenedRightAssnProperties)
                    {
                        EntityPropertyChangedEventManager.RemoveListener(assn, this._rightAssnChangedListener, propertyName);
                    }
                }

                EntityStateChangedEventManager.RemoveListener(@class, _classDeletedListener);

            }
            return true;
        }

        private bool PropertyCollectionChanged(Type managerType, object sender, EventArgs e)
        {

            NotifyCollectionChangedEventArgs args = (NotifyCollectionChangedEventArgs)e;
            if (args.Action != NotifyCollectionChangedAction.Move)
            {
                if (args.OldItems != null)
                {
                    foreach (BusinessProperty property in args.OldItems)
                    {
                        foreach (string propertyName in ListenedPropertyProperties)
                        {
                            EntityPropertyChangedEventManager.RemoveListener(property, this._classChangedListener, propertyName);
                        }
                    }
                }
                if (args.NewItems != null)
                {
                    foreach (BusinessProperty property in args.NewItems)
                    {
                        foreach (string propertyName in ListenedPropertyProperties)
                        {
                            EntityPropertyChangedEventManager.AddListener(property, this._classChangedListener, propertyName);
                        }
                    }
                }
            }

            IObservableList<BusinessProperty> collection = (IObservableList<BusinessProperty>)sender;
            BusinessClass @class = this._listenedCollections[collection];
            UpdateAutoScript(@class);
            return true;
        }

        private bool ClassChanged(Type managerType, object sender, EventArgs e)
        {
            BusinessClass @class;
            if (sender is BusinessProperty)
            {
                @class = ((BusinessProperty)sender).Class;
            }
            else
            {
                @class = (BusinessClass)sender;

                EntityPropertyChangedEventArgs args = (EntityPropertyChangedEventArgs)e;
                if (args.PropertyName == "CodeName" || args.PropertyName == "Package")
                {
                    this.Rename(@class);
                }


            }
            UpdateAutoScript(@class);

            return true;

        }

        public void UpdateAutoScript(BusinessClass @class)
        {

            if (@class.Package != null && @class.ParentClass != null)
            {
                IT4Service t4 = this.Site.GetRequiredService<IT4Service>();
                CompilerErrorCollection errors;
                string output = t4.ProcessTemplateFile(Settings.ExtractToFullPath(Settings.Default.BusinessClassAutoScriptT4Path), @class, out errors);

                @class.AutoScript = output;

                foreach (CompilerError error in errors)
                {
                    Debug.WriteLine(error.ToString());
                }
            }

            //TODO: Write Error to Error List
        }

        public void Rename(BusinessClass @class)
        {
            if (@class.Package != null && @class.ParentClass != null && @class.EntityState != EntityState.Deleted)
            {
                if (@class.Script == null)
                {
                    IT4Service t4 = this.Site.GetRequiredService<IT4Service>();
                    CompilerErrorCollection errors;
                    string output = t4.ProcessTemplateFile(Settings.ExtractToFullPath(Settings.Default.NewBusinessClassT4Path), @class, out errors);

                    @class.Script = output;

                    foreach (CompilerError error in errors)
                    {
                        Debug.WriteLine(error.ToString());
                    }
                }
                else
                {
                    IRefactoryService refactory = this.Site.GetRequiredService<IRefactoryService>();
                    string content = refactory.RenameClass(@class.Script, @class.CodeName);
                    content = refactory.RenameNamespace(content, @class.Package.FullCodeName);
                    @class.Script = content;

                    IEntityService es = this.Site.GetRequiredService<IEntityService>();
                    IBusinessDataTypeRepository dts = this.Site.GetRequiredService<IBusinessDataTypeRepository>();
                    BusinessDataType classType = dts.Class;

                    var q1 = @class.ChildClasses
                        .Union(from assn in @class.LeftAssociations select assn.RightClass)
                        .Union(from assn in @class.RightAssociations select assn.LeftClass)
                        .Union(from cls in es.GetRootClass().Flatten(c => c.ChildClasses)
                               from prop in cls.Properties
                               where prop.DataType == classType && prop.DataClassType == @class
                               select prop.Class)
                        .Distinct().Except(new BusinessClass[] { @class });
                    foreach (BusinessClass relative in q1.ToArray())
                    {
                        this.UpdateAutoScript(relative);
                    }
                }

            }
        }

    }
}
