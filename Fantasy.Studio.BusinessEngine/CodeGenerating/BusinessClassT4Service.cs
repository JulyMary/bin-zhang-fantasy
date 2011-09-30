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
        private Dictionary<IObservableList<BusinessProperty>, BusinessClass> _listenedCollections = new Dictionary<IObservableList<BusinessProperty>, BusinessClass>();
        private string _templateFileName = Settings.ExtractToFullPath(Settings.Default.BusinessClassT4FilePath);
        private string _templateContent;

        public override void InitializeService()
        {
            _templateFileName = Settings.ExtractToFullPath(Settings.Default.BusinessClassT4FilePath);
            _templateContent = LongPathFile.ReadAllText(_templateFileName);
            this._classChangedListener = new WeakEventListener(this.ClassChanged);
            this._classDeletedListener = new WeakEventListener(this.ClassDeleted);
            this._propertyCollectionChangedListener = new WeakEventListener(this.PropertyCollectionChanged);
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

            EntityStateChangedEventManager.AddListener(@class, _classDeletedListener);  
        }

        private static readonly string[] ListenedClassProperties = new string[] {"CodeName", "Package", "ParentClass"};
        private static readonly string[] ListenedPropertyProperties = new string[] {"CodeName", "DataTypeName", "IsNullable", "IsCalculated"};  



        private WeakEventListener _classChangedListener;
        private WeakEventListener _classDeletedListener;
        private WeakEventListener _propertyCollectionChangedListener;
        //private 
       

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

                EntityStateChangedEventManager.RemoveListener(@class, _classDeletedListener); 
 
            }
            return true;
        }

        private bool PropertyCollectionChanged(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(CollectionChangedEventManager))
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
