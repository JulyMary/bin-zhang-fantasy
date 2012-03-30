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
using Fantasy.Studio.Services;
using Fantasy.Adaption;
using Fantasy.BusinessEngine.Services;
using ICSharpCode.AvalonEdit.Highlighting;
using Fantasy.IO;
using System.Text.RegularExpressions;

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


        private SelectionService _selection = new SelectionService(null);

      

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            if (this.Site != null)
            {
                IMonitorSelectionService monitor = this.Site.GetRequiredService<IMonitorSelectionService>();
                monitor.CurrentSelectionService = this._selection;
            }
        }

        #region IDocumentEditingPanel Members

        public void Initialize()
        {
            
        }

        private IEntityScript _entityScript;

        private WeakEventListener _entityScriptListener;

        public void Load(object document)
        {
            this._entityScript = this.Site.GetRequiredService<IAdapterManager>().GetAdapter<IEntityScript>(document);
            this.DirtyState = this._entityScript.DirtyState;
            this.csEditor.TextDocument.Text = this._entityScript.Content ?? string.Empty;
            this.IsContentReadOnly  = this.IsReadOnly ||  this._entityScript.IsReadOnly;
            this.FindHighlight();
            _entityScriptListener = new WeakEventListener((t, sender, e)=>
                {
                    switch (((PropertyChangedEventArgs)e).PropertyName )
                    {
                        case "Name":
                            this.FindHighlight();
                            return true;
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
                        case "DirtyState":
                            this.DirtyState = this._entityScript.DirtyState;
                            return true;
                        default:
                            return false;
                    }
                });

            PropertyChangedEventManager.AddListener(this._entityScript, this._entityScriptListener, "Name");
            PropertyChangedEventManager.AddListener(this._entityScript, this._entityScriptListener, "Content");
            PropertyChangedEventManager.AddListener(this._entityScript, this._entityScriptListener, "IsReadOnly");
            PropertyChangedEventManager.AddListener(this._entityScript, this._entityScriptListener, "DirtyState");
            
            this.csEditor.TextDocument.TextChanged += new EventHandler(TextDocumentTextChanged);

            this._selection.SetSelectedComponents(new object[] { document });
        }

        private Regex _xmlDeclarationRegex = new Regex(@"^<\?xml[^\?^>]*\?>");

        private void FindHighlight()
        {
            string ext = LongPath.GetExtension(this._entityScript.Name);
            IHighlightingDefinition hd = HighlightingManager.Instance.GetDefinitionByExtension(ext);
            if (hd == null && this._entityScript.Content != null && _xmlDeclarationRegex.IsMatch(this._entityScript.Content))
            {
                hd = HighlightingManager.Instance.GetDefinition("XML");
            }
            this.SyntaxHighlighting = hd;
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


            IEntityService es = this.Site.GetRequiredService<IEntityService>();

            es.BeginUpdate();
            try
            {
                es.SaveOrUpdate(this._entityScript.Entity);
                es.EndUpdate(true);
            }
            catch
            {
                es.EndUpdate(false);
                throw;
            }
         
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



        public IHighlightingDefinition SyntaxHighlighting
        {
            get { return (IHighlightingDefinition)GetValue(SyntaxHighlightingProperty); }
            set { SetValue(SyntaxHighlightingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SyntaxHighlighting.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SyntaxHighlightingProperty =
            DependencyProperty.Register("SyntaxHighlighting", typeof(IHighlightingDefinition), typeof(EntityScriptEditingPanel), new UIPropertyMetadata(null));

    }
}
