using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Fantasy.ServiceModel
{
    public class TaskBarProgressMonitor : IProgressMonitor
    {

        TaskbarProgressBarState _state = TaskbarProgressBarState.Normal;
        private bool _supported = TaskbarManager.IsPlatformSupported;
        private int _value = 0;
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    this._value = value;
                    this.SetTaskBar();
                }
            }
        }

        private void SetTaskBar()
        {
            if (_supported)
            {
                TaskbarProgressBarState newState;
                if (this.Style == ProgressMonitorStyle.Blocks)
                {
                    if (this.Value == this.Minimum)
                    {
                        newState = TaskbarProgressBarState.NoProgress;
                    }
                    else
                    {
                        newState = TaskbarProgressBarState.Normal;
                    }

                }
                else
                {
                    newState = TaskbarProgressBarState.Indeterminate;
                }

                if (newState != _state)
                {
                    _state = newState;
                    TaskbarManager.Instance.SetProgressState(_state);
                }
                if (_state == TaskbarProgressBarState.Normal)
                {
                    TaskbarManager.Instance.SetProgressValue(this.Value - this.Minimum, this.Maximum - this.Minimum);
                }
            }
        }


        private int _maximum = 100;
        public int Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                if (_maximum != value)
                {
                    this._maximum = value;
                    this.SetTaskBar();
                }
            }
        }

        private int _minimum = 0;
        public int Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                if (_minimum != value)
                {
                    _minimum = value;
                    this.SetTaskBar();
                }
            }
        }

        private ProgressMonitorStyle _style = ProgressMonitorStyle.Blocks;
        public ProgressMonitorStyle Style
        {
            get
            {
                return _style;
            }
            set
            {
                if (_style != value)
                {
                    _style = value;
                    this.SetTaskBar();
                }
            }
        }

       
    }
}
