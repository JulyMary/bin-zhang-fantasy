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

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public class BusinessClassT4Service : ServiceBase, IBusinessClassT4Service
    {
        private Dictionary<object, BusinessClass> _listenedCollections = new Dictionary<object, BusinessClass>();
        private string _templateFileName = Settings.ExtractToFullPath(Settings.Default.BusinessClassT4FilePath);
        private string _templateContent;

        public override void InitializeService()
        {
            _templateFileName = Settings.ExtractToFullPath(Settings.Default.BusinessClassT4FilePath);
            _templateContent = LongPathFile.ReadAllText(_templateFileName);
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
                PropertyChangedEventManager.AddListener(@class, this._classChangedListener, propertyName);
            }

           
            CollectionChangedEventManager.AddListener(@class.Properties, this._propertyCollectionChangedListener);
            this._listenedCollections.Add(@class.Properties, @class);
            

            foreach (BusinessProperty property in @class.Properties)
            {
                foreach (string propertyName in ListenedPropertyProperties)
                {
                    PropertyChangedEventManager.AddListener(property, this._classChangedListener, propertyName);
                }
            }


            CollectionChangedEventManager.AddListener(@class.LeftAssociations, this._leftAssnCollectionChangedListener);
            this._listenedCollections.Add(@class.LeftAssociations, @class);

            foreach (BusinessAssociation assn in @class.LeftAssociations)
            {
                foreach (string propertyName in ListenedLeftAssnProperties)
                {
                    PropertyChangedEventManager.AddListener(assn, this._leftAssnChangedListener, propertyName); 
                }
            }


            CollectionChangedEventManager.AddListener(@class.RightAssociations, this._rightAssnCollectionChangedListener);
            this._listenedCollections.Add(@class.RightAssociations, @class);
            foreach (BusinessAssociation assn in @class.RightAssociations)
            {
                foreach (string propertyName in ListenedRightAssnProperties)
                {
                    PropertyChangedEventManager.AddListener(assn, this._rightAssnChangedListener, propertyName);
                }
            }


            EntityStateChangedEventManager.AddListener(@class, _classDeletedListener);  
        }

        private static readonly string[] ListenedClassProperties = new string[] {"CodeName", "Package", "ParentClass"};
        private static readonly string[] ListenedPropertyProperties = new string[] {"CodeName", "DataTypeName", "IsNullable", "IsCalculated"};
        private static readonly string[] ListenedLeftAssnProperties = new string[] { "RightRoleCode", "RightCardinality" };
        private static readonly string[] ListenedRightAssnProperties = new string[] { "LeftRoleCode", "LeftCardinality" };



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
            this.UpdateScript(assn.LeftClass);
            return true;
        }

        private bool RightAssnChanged(Type managerType, object sender, EventArgs e)
        {
            BusinessAssociation assn = (BusinessAssociation)sender;
            this.UpdateScript(assn.RightClass);
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
                            PropertyChangedEventManager.RemoveListener(assn, this._leftAssnChangedListener, propertyName);
                        }
                    }
                }
                if (args.NewItems != null)
                {
                    foreach (BusinessProperty assn in args.NewItems)
                    {
                        foreach (string propertyName in ListenedLeftAssnProperties)
                        {
                            PropertyChangedEventManager.AddListener(assn, this._leftAssnChangedListener, propertyName);
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
                            PropertyChangedEventManager.RemoveListener(assn, this._rightAssnChangedListener, propertyName);
                        }
                    }
                }
                if (args.NewItems != null)
                {
                    foreach (BusinessProperty assn in args.NewItems)
                    {
                        foreach (string propertyName in ListenedRightAssnProperties)
                        {
                            PropertyChangedEventManager.AddListener(assn, this._rightAssnChangedListener, propertyName);
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
                    PropertyChangedEventManager.RemoveListener(@class, this._classChangedListener, propertyName);
                }

                CollectionChangedEventManager.RemoveListener(@class.Properties, this._propertyCollectionChangedListener);
                this._listenedCollections.Remove(@class.Properties);

                foreach (BusinessProperty property in @class.Properties)
                {
                    foreach (string propertyName in ListenedPropertyProperties)
                    {
                        PropertyChangedEventManager.RemoveListener(property, this._classChangedListener, propertyName);
                    }
                }


                CollectionChangedEventManager.RemoveListener(@class.LeftAssociations, this._leftAssnCollectionChangedListener);
                this._listenedCollections.Remove(@class.LeftAssociations);

                foreach (BusinessAssociation assn in @class.LeftAssociations)
                {
                    foreach (string propertyName in ListenedLeftAssnProperties)
                    {
                        PropertyChangedEventManager.RemoveListener(assn, this._leftAssnChangedListener, propertyName);
                    }
                }


                CollectionChangedEventManager.AddListener(@class.RightAssociations, this._rightAssnCollectionChangedListener);
                this._listenedCollections.Remove(@class.RightAssociations);
                foreach (BusinessAssociation assn in @class.RightAssociations)
                {
                    foreach (string propertyName in ListenedRightAssnProperties)
                    {
                        PropertyChangedEventManager.RemoveListener(assn, this._rightAssnChangedListener, propertyName);
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
                            PropertyChangedEventManager.RemoveListener(property, this._classChangedListener, propertyName);
                        }
                    }
                }
                if (args.NewItems != null)
                {
                    foreach (BusinessProperty property in args.NewItems)
                    {
                        foreach (string propertyName in ListenedPropertyProperties)
                        {
                            PropertyChangedEventManager.AddListener(property, this._classChangedListener, propertyName);
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
            }
            UpdateAutoScript(@class);
            
            return true;

        }

        public void UpdateAutoScript(BusinessClass @class)
        {

            if (@class.Package != null && @class.ParentClass != null)
            {
                ServiceContainer site = new ServiceContainer(this.Site);
                site.AddService(@class);
                T4EngineHost host = new T4EngineHost()
                {
                    Site = site,
                    TemplateFile = this._templateFileName,
                };

                Engine engine = new Engine();

                string output = engine.ProcessTemplate(this._templateContent, host);

                @class.AutoScript = output;

                foreach (CompilerError error in host.CompilerErrors)
                {
                    Debug.WriteLine(error.ToString());
                }
            }

            //TODO: Write Error to Error List
        }

        #region IBusinessClassT4Service Members


        public void UpdateScript(BusinessClass @class)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
