using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Collections.ObjectModel;
using Fantasy.AddIns;
using Fantasy.Studio.BusinessEngine.Properties;
using Fantasy.Windows;
using Fantasy.ComponentModel;
using Fantasy.BusinessEngine;
using Fantasy.Web;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    class BusinessApplicationViewDesignPanelModel : NotifyPropertyChangedObject, IObjectWithSite
    {
        private ObservableCollection<ApplicationView> _views;
        public ObservableCollection<ApplicationView> Views
        {
            get
            {
                return _views;
            }
        }

        public BusinessApplicationViewDesignPanelModel(BusinessApplication app, IServiceProvider site)
        {
            this.Entity = app;
            this.Site = site;
            this._views = new ObservableCollection<ApplicationView>(AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/application/wellknownviews").BuildChildItems<ApplicationView>(this, this.Site));

            //this.BrowseButton = new ApplicationView()
            //{
            //    Caption = Resources.BrowseApplicationViewText,
            //    Description = Resources.BrowseApplicationViewDescription,
            //};
            //this._views.Add(BrowseButton);
        }

        public IServiceProvider Site { get; set; }

        public BusinessApplication Entity { get; private set; }


       // public ApplicationView BrowseButton {get;private set;}

        private ApplicationView _selectedView;

        public ApplicationView SelectedView
        {
            get { return _selectedView; }
            set
            {
                if (_selectedView != value)
                {
                    _selectedView = value;
                    this.OnPropertyChanged("SelectedView");

                    UpdateDesigner();
                }
            }
        }

        private void UpdateDesigner()
        {
            if (_selectedView != null && typeof(ICustomerizableViewController).IsAssignableFrom(_selectedView.Type))
            {

                Type designerType;
                EditorAttribute ea = _selectedView.Type.GetCustomAttribute<EditorAttribute>(required: false);
                if (ea == null)
                {
                    designerType = ea.EditorType;
                }
                else
                {
                    designerType = typeof(DefaultApplicationViewDesigner);
                }

                IViewDesigner designer = (IViewDesigner)Activator.CreateInstance(designerType);
                if (designer is IObjectWithSite)
                {
                    ((IObjectWithSite)designer).Site = this.Site;
                }




                
                

            }
        }

        private IViewDesigner _viewDesigner;

        public IViewDesigner ViewDesigner
        {
            get { return _viewDesigner; }
            set
            {
                if (_viewDesigner != value)
                {
                    _viewDesigner = value;
                    this.OnPropertyChanged("ViewDesigner");
                }
            }
        }
    }


   
}
