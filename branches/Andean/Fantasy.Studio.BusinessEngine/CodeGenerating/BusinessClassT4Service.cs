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

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public class BusinessClassT4Service : ServiceBase, Fantasy.Studio.BusinessEngine.CodeGenerating.IBusinessClassT4Service
    {
        private List<BusinessClass> _listenedClasses = new List<BusinessClass>();
        private string _templateFileName = Settings.ExtractToFullPath(Settings.Default.BusinessClassT4FilePath);
        private string _templateContent;

        public override void InitializeService()
        {
            _templateFileName = Settings.ExtractToFullPath(Settings.Default.BusinessClassT4FilePath);
            _templateContent = LongPathFile.ReadAllText(_templateFileName);
            this._classChangedListener = new WeakEventListener(this.ClassChanged);
            base.InitializeService();
        }

        public void RegisterClass(Fantasy.BusinessEngine.BusinessClass @class)
        {
            foreach (string propertyName in ListenedClassProperties)
            {
                PropertyChangedEventManager.AddListener(@class, this._classChangedListener, propertyName);
            }

            CollectionChangedEventManager.AddListener(@class.Properties, this._classChangedListener);

            foreach (BusinessProperty property in @class.Properties)
            {
                foreach (string propertyName in ListenedPropertyProperties)
                {
                    PropertyChangedEventManager.AddListener(property, this._classChangedListener, propertyName);
                }
            }
        }

        private static readonly string[] ListenedClassProperties = new string[] {"CodeName"};
        private static readonly string[] ListenedPropertyProperties = new string[] {"CodeName", "DataTypeName", "IsNullable", "IsCalculated"};  



        private WeakEventListener _classChangedListener;



        private bool ClassChanged(Type managerType, object sender, EventArgs e)
        {

            if (managerType == typeof(CollectionChangedEventManager))
            {
                NotifyCollectionChangedEventArgs  args = (NotifyCollectionChangedEventArgs)e;
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

            BusinessClass @class = (BusinessClass)sender;
            UpdateAutoScript(@class);
            return true;

        }

        public void UpdateAutoScript(BusinessClass @class)
        {
            ServiceContainer site = new ServiceContainer(this.Site);
            site.AddService(@class);
            BusinessClassT4EngineHost host = new BusinessClassT4EngineHost()
            {
                Site = site,
                TemplateFile = this._templateFileName,
            };

            Engine engine = new Engine();

            string output = engine.ProcessTemplate(this._templateContent, host);
           
            @class.AutoScript = output;

            //TODO: Write Error to Error List
        }
    }
}
