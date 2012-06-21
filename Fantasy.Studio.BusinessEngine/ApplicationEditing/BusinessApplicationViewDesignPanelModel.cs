using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Collections.ObjectModel;
using Fantasy.AddIns;
using Fantasy.Studio.BusinessEngine.Properties;
using Fantasy.Windows;

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

        public BusinessApplicationViewDesignPanelModel(IServiceProvider site)
        {
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
