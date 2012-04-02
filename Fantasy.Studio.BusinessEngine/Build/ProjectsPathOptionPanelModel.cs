using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.Studio.BusinessEngine.Properties;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class ProjectsPathOptionPanelModel : NotifyPropertyChangedObject
    {
        public ProjectsPathOptionPanelModel()
        {
            this._solutionPath = Settings.Default.SolutionPath;

        }


        private string _solutionPath;

        public string SolutionPath
        {
            get { return _solutionPath; }
            set
            {
                if (_solutionPath != value)
                {
                    _solutionPath = value;
                    this.OnPropertyChanged("SolutionPath");
                }
            }
        }

       
        public void Save()
        {
            Settings.Default.SolutionPath = this._solutionPath;
           
        }
    }
}
