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
using Fantasy.Studio.Controls;
using Fantasy.Windows;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    /// <summary>
    /// Interaction logic for FontAndColorsOptionPanel.xaml
    /// </summary>
    public partial class FontAndColorsOptionPanel : UserControl, IOptionPanel
    {
        public FontAndColorsOptionPanel()
        {
            InitializeComponent();
            this.DataContext = this._model;
            this._model.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ModelChanged);

           
        }

        void ModelChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.DirtyState = EditingState.Dirty; 
        }

        private FontAndColorsOptionPanelModel _model = new FontAndColorsOptionPanelModel();

        #region IOptionPanel Members

        public void Save()
        {
            Properties.Settings.Default.CodeEditorFontFamily = _model.FontFamily.Source;
            Properties.Settings.Default.CodeEditorFontSize = _model.Size;
            this.DirtyState = EditingState.Clean;
        }

        private EditingState _dirtyState = EditingState.Clean; 
        public EditingState DirtyState
        {
            get
            {
                return _dirtyState;
            }
            set
            {
                if (this._dirtyState != value)
                {
                    this._dirtyState = value;
                    if (this.DirtyStateChanged != null)
                    {
                        this.DirtyStateChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event EventHandler DirtyStateChanged;

        #endregion


       

    }
}
