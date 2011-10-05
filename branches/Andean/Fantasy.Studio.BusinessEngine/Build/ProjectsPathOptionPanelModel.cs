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
            this._runtimeAssemblyName = Settings.Default.RuntimeAssemblyName;
            this._webAssemblyName = Settings.Default.WebAssemblyName;
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

        private string _runtimeAssemblyName;

        public string RuntimeAssemblyName
        {
            get { return _runtimeAssemblyName; }
            set
            {
                if (_runtimeAssemblyName != value)
                {
                    _runtimeAssemblyName = value;
                    this.OnPropertyChanged("RuntimeAssemblyName");
                }
            }
        }

        private string _webAssemblyName;

        public string WebAssemblyName
        {
            get { return _webAssemblyName; }
            set
            {
                if (_webAssemblyName != value)
                {
                    _webAssemblyName = value;
                    this.OnPropertyChanged("WebAssemblyName");
                }
            }
        }

        public void Save()
        {
            Settings.Default.SolutionPath = this._solutionPath;
            Settings.Default.RuntimeAssemblyName = this._runtimeAssemblyName;
            Settings.Default.WebAssemblyName = this._webAssemblyName;
        }
    }
}
