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
using Microsoft.Win32;

namespace Fantasy.Studio.BusinessEngine.Build
{
    /// <summary>
    /// Interaction logic for ProjectsPathOptionPanel.xaml
    /// </summary>
    public partial class ProjectsPathOptionPanel : UserControl, IOptionPanel
    {
        public ProjectsPathOptionPanel()
        {
            InitializeComponent();
            this.DataContext = this._model;
            this._model.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ModelPropertyChanged);

        }

        void ModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.DirtyState = EditingState.Dirty; 
        }

        private ProjectsPathOptionPanelModel _model = new ProjectsPathOptionPanelModel();

        #region IOptionPanel Members

        public void Save()
        {
            _model.Save();
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

        private void BrowsSolutionButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".sln"; // Default file extension
            dlg.Filter = Properties.Resources.SolutionFileDialogFilter; // Filter files by extension
            dlg.Title = Properties.Resources.SolutionFileDialogTitle;
            dlg.FileName = this._model.SolutionPath;
            dlg.AddExtension = true;
            if ((bool)dlg.ShowDialog())
            {
                this._model.SolutionPath = dlg.FileName;
            }
        }

      
    }
}
