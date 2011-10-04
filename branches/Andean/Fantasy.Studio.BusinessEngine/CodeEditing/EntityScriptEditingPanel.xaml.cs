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
using Fantasy.Windows;
using System.ComponentModel;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    /// <summary>
    /// Interaction logic for EntityScriptEditingPanel.xaml
    /// </summary>
    public partial class EntityScriptEditingPanel : UserControl, IObjectWithSite, IDocumentEditingPanel
    {
        public EntityScriptEditingPanel()
        {
            InitializeComponent();
        }

        public IServiceProvider Site { get; set; }

        #region IDocumentEditingPanel Members

        public void Initialize()
        {
            
        }

        private IEntityScript _entityScript;

        private WeakEventListener _entityScriptListener;

        public void Load(object document)
        {
            this._entityScript = (IEntityScript)document;
            this.csEditor.TextDocument.Text = this._entityScript.Content ?? string.Empty;
            this.IsContentReadOnly  = this.IsReadOnly ||  this._entityScript.IsReadOnly;
            _entityScriptListener = new WeakEventListener((t, sender, e)=>
                {
                    switch (((PropertyChangedEventArgs)e).PropertyName )
                    {
                        case "Content":
                            if (!this._syncing)
                            {
                                this._syncing = true;
                                try
                                {
                                    this.csEditor.TextDocument.Text = this._entityScript.Content ?? String.Empty;
                                }
                                finally
                                {
                                    this._syncing = false;
                                }
                            }
                            return true;
                        case "IsReadOnly":
                            this.IsContentReadOnly = this.IsReadOnly || this._entityScript.IsReadOnly;
                            return true;

                        default:
                            return false;
                    }
                });


            PropertyChangedEventManager.AddListener(this._entityScript, this._entityScriptListener, "Content");
            PropertyChangedEventManager.AddListener(this._entityScript, this._entityScriptListener, "IsReadOnly");
            
            this.csEditor.TextDocument.TextChanged += new EventHandler(TextDocumentTextChanged);
        }

       
        void TextDocumentTextChanged(object sender, EventArgs e)
        {
            if (!this._syncing)
            {
                this._syncing = true;

                try
                {
                    this._entityScript.Content = this.csEditor.TextDocument.Text;
                    this.DirtyState = EditingState.Dirty;
                }
                finally
                {
                    this._syncing = false;
                }
            }
           
        }


        private bool _syncing = false;


        private EditingState _dirtyState = EditingState.Clean;
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

        public void Save()
        {
            
            this.DirtyState = EditingState.Clean;
        }

        public UIElement Element
        {
            get { return this; }
        }

        public string Title
        {
            get { return Properties.Resources.DefaultCodePanelTitle; }
        }

        public void Closing(System.ComponentModel.CancelEventArgs e)
        {
            
        }

        public void Closed()
        {
            this.csEditor.TextDocument.TextChanged -= new EventHandler(TextDocumentTextChanged);
        }

        public void ViewContentSelected()
        {
           
        }

        public void ViewContentDeselected()
        {
           
        }

        #endregion



        public bool IsContentReadOnly
        {
            get { return (bool)GetValue(IsContentReadOnlyPropertyKey.DependencyProperty); }
            private set { SetValue(IsContentReadOnlyPropertyKey, value); }
        }

        // Using a DependencyProperty as the backing store for IsContentReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey IsContentReadOnlyPropertyKey =
            DependencyProperty.RegisterReadOnly("IsContentReadOnly", typeof(bool), typeof(EntityScriptEditingPanel), new UIPropertyMetadata(false));



        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(EntityScriptEditingPanel), new FrameworkPropertyMetadata(false, IsReadOnlyChangedCallback));

        private static void IsReadOnlyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntityScriptEditingPanel panel = ((EntityScriptEditingPanel)d);
            ((EntityScriptEditingPanel)d).IsContentReadOnly =  (bool)e.NewValue || panel._entityScript.IsReadOnly;  
        }
 

    }
}
