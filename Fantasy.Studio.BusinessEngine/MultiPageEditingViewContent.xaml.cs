using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Fantasy.BusinessEngine;
using Fantasy.AddIns;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Fantasy.Studio.BusinessEngine
{
    /// <summary>
    /// Interaction logic for EntityEditingViewContent.xaml
    /// </summary>
    public abstract partial class MultiPageEditingViewContent : UserControl, IViewContent, IEditingViewContent, IObjectWithSite, IDocumentEditingPanelContainer
    {
        public MultiPageEditingViewContent()
        {
            InitializeComponent();
            this.Panels = new ObservableCollection<IDocumentEditingPanel>();
        }

        public abstract string EditingPanelPath { get; }

        public abstract string CommandBindingPath { get; }
            

      

        public ICollection<IDocumentEditingPanel> Panels
        {
            get { return (ICollection<IDocumentEditingPanel>)GetValue(PanelsProperty); }
            private set { SetValue(PanelsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Panels.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PanelsProperty =
            DependencyProperty.Register("Panels", typeof(ICollection<IDocumentEditingPanel>), typeof(EntityEditingViewContent), new UIPropertyMetadata(null));




        public virtual void Load(object data)
        {
            this.Data = data;
            
           
            if (this.EditingPanelPath != null)
            {
                foreach (IDocumentEditingPanel panel in AddInTree.Tree.GetTreeNode(this.EditingPanelPath).BuildChildItems<IDocumentEditingPanel>(this, this.Site))
                {
                    this.Panels.Add(panel);
                   
                }
               
            }

            if (this.CommandBindingPath != null)
            {
                foreach (CommandBinding cb in AddInTree.Tree.GetTreeNode(this.CommandBindingPath).BuildChildItems(this, this.Site))
                {
                    this.CommandBindings.Add(cb);
                }
            }

            foreach (IDocumentEditingPanel panel in this.Panels)
            {
                panel.Initialize();
                panel.Load(this.Data);
                panel.DirtyStateChanged += new EventHandler(EditingPanel_DirtyStateChanged);
            }


            

            EvalDirtyState();

            this.panelContainer.DataContext = this;
            if (this.Panels.Count > 0)
            {
                this.ActivePanel = this.Panels.First();
                this.panelContainer.SelectedIndex = 0;
            }
        }

        public IDocumentEditingPanel ActivePanel
        {
            get { return (IDocumentEditingPanel)GetValue(ActivePanelProperty); }
            set { SetValue(ActivePanelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActivePanel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActivePanelProperty =
            DependencyProperty.Register("ActivePanel", typeof(IDocumentEditingPanel), typeof(EntityEditingViewContent), new UIPropertyMetadata(null, ActivePanelChangedCallback));

        private static void ActivePanelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EntityEditingViewContent)d).panelContainer.SelectedItem = e.NewValue;

            ((EntityEditingViewContent)d).OnActivePanelChanged(EventArgs.Empty);
        }

        private void panelContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ActivePanel = (IDocumentEditingPanel)this.panelContainer.SelectedItem;
        }


        public event EventHandler ActivePanelChanged;

        protected virtual void OnActivePanelChanged(EventArgs e)
        {
            if (this.ActivePanelChanged != null)
            {
                this.ActivePanelChanged(this, e);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }



        void EditingPanel_DirtyStateChanged(object sender, EventArgs e)
        {
            EvalDirtyState();
        }

        private void EvalDirtyState()
        {
            EditingState  state = EditingState.Clean;
            foreach (IDocumentEditingPanel panel in this.Panels)
            {
                if (panel.DirtyState == EditingState.Dirty)
                {
                    state = EditingState.Dirty;
                    break;
                }
            }
            this.DirtyState = state;
        }

        void Entity_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                this.OnTitleChanged(EventArgs.Empty);
            }
        }


        #region IViewContent Members

        public UIElement Element
        {
            get { return this; }
        }

        IWorkbenchWindow IViewContent.WorkbenchWindow { get; set; }


        private string _title;

        public virtual string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    this.OnTitleChanged(EventArgs.Empty);
                }
            }
        }

       

        public virtual void Selected()
        {

            foreach (IDocumentEditingPanel panel in this.Panels)
            {
                panel.ViewContentSelected();
            }
        }

        public virtual void Deselected()
        {
            foreach (IDocumentEditingPanel panel in this.Panels)
            {
                panel.ViewContentDeselected();
            }
        }

        public virtual void Closing(System.ComponentModel.CancelEventArgs e)
        {
            foreach (IDocumentEditingPanel panel in this.Panels)
            {
                panel.Closing(e);
            }
        }

        public virtual void Closed()
        {
            foreach (IDocumentEditingPanel panel in this.Panels)
            {
                panel.Closed();
            }
        }

        public event EventHandler TitleChanged;

        protected virtual void OnTitleChanged(EventArgs e)
        {
            if (this.TitleChanged != null)
            {
                this.TitleChanged(this, e);
            }
        }


        public abstract string DocumentName { get; }
       

        public abstract string DocumentType {get;} 
       
        public virtual void Dispose()
        {
            
        }

        #endregion

        #region IEditingViewContent Members

        

        public object Data { get; private set; }
       

        private EditingState _dirtyState =  EditingState.Clean;
        public virtual EditingState DirtyState
        {
            get
            {
                return _dirtyState;
            }
            protected set
            {
                if (_dirtyState != value)
                {
                    this._dirtyState = value;
                    this.OnDirtyStateChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler DirtyStateChanged;

        protected virtual void OnDirtyStateChanged(EventArgs e)
        {
            if (this.DirtyStateChanged != null)
            {
                this.DirtyStateChanged(this, e);
            }
        }


        public virtual void Save()
        {
            foreach (IDocumentEditingPanel panel in this.Panels)
            {
                if (panel.DirtyState == EditingState.Dirty)
                {
                    panel.Save();
                }
            }
            //this.EvalDirtyState();
        }

        #endregion

        public IServiceProvider Site { get; set; }


        private ImageSource _icon;

        public virtual ImageSource Icon
        {
            get { return _icon; }
            set 
            {  
                
                if (this._icon != value)
                {
                    this._icon = value;
                    this.OnIconChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler IconChanged;

        protected virtual void OnIconChanged(EventArgs e)
        {
            if (this.IconChanged != null)
            {
                this.IconChanged(this, e);
            }
        }


       



       
       
    }
}
