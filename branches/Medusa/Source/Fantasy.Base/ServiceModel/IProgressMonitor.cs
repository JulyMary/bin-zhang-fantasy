using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Fantasy.Properties;

namespace Fantasy.ServiceModel
{

    public enum ProgressMonitorStyle
    {  
        Blocks = 0,
        Continuous = 1,
        Marquee = 2,
    };

    public interface IProgressMonitor
    {
        int Value { get; set; }
        int Maximum { get; set; }
        int Minimum { get; set; }

        ProgressMonitorStyle Style { get; set; }
    }

    public interface IProgressMonitorContainer : IProgressMonitor
    {
        void AddMoniter(IProgressMonitor monitor);
        void RemoveMoniter(IProgressMonitor monitor);
        void Increment(int value);
        int Step { get; set; }

        void PerformStep();
    }

    public class ProgressBarMonitor : IProgressMonitor
    {

        private ProgressBar _control;
        public ProgressBarMonitor(ProgressBar control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            this._control = control;
            if (!this._control.IsHandleCreated)
            {
                this._control.HandleCreated += new EventHandler(Control_HandleCreated);
            }
        }

        void Control_HandleCreated(object sender, EventArgs e)
        {
            this._control.HandleCreated -= new EventHandler(Control_HandleCreated);
            this._control.Invoke(new MethodInvoker(() => {
                switch (this.Style)
                {
                    case ProgressMonitorStyle.Blocks:
                        this._control.Style = ProgressBarStyle.Blocks;
                        break;
                    case ProgressMonitorStyle.Continuous:
                        this._control.Style = ProgressBarStyle.Continuous; 
                        break;
                    case ProgressMonitorStyle.Marquee:
                        this._control.Style = ProgressBarStyle.Marquee; 
                        break;
                  
                }
                this._control.Minimum = this.Minimum;
                this._control.Maximum = this.Maximum;
                this._control.Value = this.Value;
            }));

            
        }

        private int _maximum = 100;
        public int Maximum
        {
            get { return _maximum; }

            set
            {
                this._maximum = value;
                if (this._control.IsHandleCreated)
                {
                    
                    if ((this._control.InvokeRequired))
                    {
                        MethodInvoker<string, int> del = new MethodInvoker<string, int>(SetProperty);
                        this._control.Invoke(del, "maximum", value);
                    }
                    else
                    {
                        this._control.Maximum = value;
                    }
                }

            }
        }

        private void SetProperty(string name, int value)
        {
            switch (name)
            {
                case "maximum":
                    this._control.Maximum = value;
                    break;
                case "minimum":
                    this._control.Minimum = value;
                    break;
                case "value":
                    this._control.Value = value;
                    break;
            }
           
        }

        private int _minimum = 0;
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                this._minimum = value;
                if (this._control.IsHandleCreated)
                {
                    if ((this._control.InvokeRequired))
                    {
                        MethodInvoker<string, int> del = new MethodInvoker<string, int>(SetProperty);
                        this._control.Invoke(del, "minimum", value);
                    }
                    else
                    {
                        this._control.Minimum = value;
                    }
                }

            }
        }

        private int _value;

        public int Value
        {
            get { return this._value; }
            set
            {
                if (this._control.IsHandleCreated)
                {
                    this._value = value;
                    if ((this._control.InvokeRequired))
                    {
                        MethodInvoker<string, int> del = new MethodInvoker<string, int>(SetProperty);
                        this._control.Invoke(del, "value", value);
                    }
                    else
                    {
                        this._control.Value = value;
                    }
                }
            }
        }

        private ProgressMonitorStyle _style = ProgressMonitorStyle.Blocks;
        public ProgressMonitorStyle Style
        {
            get
            {
               
                return this._style;
            }
            set
            {
                this._style = value;
                if (this._control.IsHandleCreated)
                {
                    this._control.Invoke(new MethodInvoker(() =>
                    {
                        switch (value)
                        {
                            case ProgressMonitorStyle.Blocks:
                                this._control.Style = ProgressBarStyle.Blocks;
                                break;
                            case ProgressMonitorStyle.Continuous:
                                this._control.Style = ProgressBarStyle.Continuous;
                                break;
                            case ProgressMonitorStyle.Marquee:
                                this._control.Style = ProgressBarStyle.Marquee;
                                break;

                        }
                    }));
                }
            }
        }
    }

    public class ProgressMonitorContainer : IProgressMonitorContainer
    {

        private ArrayList _monitors = new ArrayList();
        private int _value = 0;
        private int _maximum = 100;
        private int _minimum = 0;

        private int _step = 1;
        public void Increment(int Value)
        {
            this.Value = this.Value + Value;
        }

        public int Maximum
        {
            get { return this._maximum; }
            set
            {
                this._maximum = value;
                foreach (IProgressMonitor monitor in this._monitors)
                {
                    monitor.Maximum = value;
                }
            }
        }

        public int Minimum
        {
            get { return this._minimum; }
            set
            {
                this._minimum = value;
                foreach (IProgressMonitor monitor in this._monitors)
                {
                    monitor.Minimum = value;
                }
            }
        }

        public int Value
        {
            get { return this._value; }
            set
            {
                if (value < this._minimum)
                {
                    this._value = this._minimum;
                }
                else if (value > this.Maximum)
                {
                    this._value = this.Maximum;
                }
                else
                {
                    this._value = value;
                }

                foreach (IProgressMonitor monitor in this._monitors)
                {
                    monitor.Value = this._value;
                }

            }
        }

        public void AddMoniter(IProgressMonitor monitor)
        {
            monitor.Maximum = this.Maximum;
            monitor.Minimum = this.Minimum;
            monitor.Value = this.Value;
            monitor.Style = this.Style;
            this._monitors.Add(monitor);
        }

        public void RemoveMoniter(IProgressMonitor monitor)
        {
            this._monitors.Remove(monitor);
        }

        public void PerformStep()
        {
            this.Increment(this._step);
        }

        public int Step
        {
            get { return this._step; }
            set { this._step = value; }
        }

        private ProgressMonitorStyle _style = ProgressMonitorStyle.Blocks; 

        public ProgressMonitorStyle Style
        {
            get { return _style; }
            set 
            {
                if (_style != value)
                {
                    _style = value;

                    foreach (IProgressMonitor monitor in this._monitors)
                    {
                        monitor.Style = _style;
                    }
                }
            }
        }


    }

    public class SequenceProgressMonitor : IProgressMonitor, IEnumerable
    {

        private IProgressMonitor _owner;

        private SubProgressMonitor[] _children;

        private class SubProgressMonitor : IProgressMonitor
        {

            private int _maximum = 100;
            private int _minimum = 0;
            private int _value = 0;

            private SequenceProgressMonitor _owner;

            public double StartPoint;

            public double Length;


            public SubProgressMonitor(SequenceProgressMonitor owner, double startPoint, double length)
            {
                this._owner = owner;
                this.StartPoint = startPoint;
                this.Length = length;
            }

            public int Maximum
            {
                get { return this._maximum; }
                set { this._maximum = value; }
            }

            public int Minimun
            {
                get { return this._minimum; }
                set { this._minimum = value; }
            }
            int IProgressMonitor.Minimum
            {
                get { return Minimun; }
                set { Minimun = value; }
            }

            public int Value
            {
                get { return this._value; }
                set
                {
                    if (value < this._minimum)
                    {
                        this._value = this._minimum;
                    }
                    else if (value > this.Maximum)
                    {
                        this._value = this.Maximum;
                    }
                    else
                    {
                        this._value = value;
                    }
                    this._owner.OnSubMonitorValueChange(this);
                }
            }

            public ProgressMonitorStyle Style
            {
                get
                {
                    return this._owner.Style;
                }
                set
                {
                    this._owner.Style = value;
                }
            }
        }

        public SequenceProgressMonitor(IProgressMonitor monitor, double[] subLengths)
        {
            this.Initialize(monitor, subLengths);
        }


        private void Initialize(IProgressMonitor monitor, double[] subLengths)
        {
            this._owner = monitor;
            double start = 0;
            this._children = (SubProgressMonitor[])Array.CreateInstance(typeof(SubProgressMonitor), subLengths.Length);
            for (int i = 0; i <= subLengths.Length - 1; i++)
            {
                SubProgressMonitor subMonitor = new SubProgressMonitor(this, start, subLengths[i]);
                this._children[i] = subMonitor;
                start += subLengths[i];
            }
            if (start != 1.0)
            {
                throw new ApplicationException(Resources.InvalidMultProgressStepsText);
            }
        }

        public SequenceProgressMonitor(IProgressMonitor monitor, int count)
        {
            double[] subLengths = (double[])Array.CreateInstance(typeof(double), count);
            double avg = Math.Round(1.0 / Convert.ToDouble(count), 4);
            double total = 0;
            for (int i = 0; i <count; i++)
            {
                if (i < count - 1)
                {
                    subLengths[i] = avg;
                    total += avg;
                }
                else
                {
                    subLengths[i] = 1.0 - total;
                }
            }

            this.Initialize(monitor, subLengths);

        }

        public IProgressMonitor this[int index]
        {
            get { return _children[index]; }
        }

        public int Count
        {
            get { return _children.Length; }
        }

        public int Maximum
        {
            get { return this._owner.Maximum; }
            set { this._owner.Maximum = value; }
        }

        public int Minimum
        {
            get { return this._owner.Minimum; }
            set { this._owner.Minimum = value; }
        }

        public int Value
        {
            get { return this._owner.Value; }
            set { this._owner.Value = value; }
        }

        public ProgressMonitorStyle Style
        {
            get { return this._owner.Style; }
            set { this._owner.Style = value; }
        }

        private void OnSubMonitorValueChange(SubProgressMonitor monitor)
        {
            double percentage = (monitor.Value - monitor.Minimun) / (monitor.Maximum - monitor.Minimun);
            double value = this.Minimum + (this.Maximum - this.Minimum) * monitor.StartPoint;
            value += (this.Maximum - this.Minimum) * monitor.Length * percentage;
            this._owner.Value = (int)Math.Round(value);
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this._children.GetEnumerator();
        }
    }

    public class ProgressTimeEvaluator : IProgressMonitor
    {

        private int _maximum = 100;
        public int Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        private int _minimum = 0;
        public int Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }
        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                if ((this._value != value))
                {
                    _value = value;
                    if (this._started)
                    {
                        int valOffset = _value - _minimum;
                        int len = this._maximum - this._minimum;
                        TimeSpan timeConsumed = this.TimeCosumed;
                        if (valOffset > 0 && len > 0 && timeConsumed != TimeSpan.Zero)
                        {
                            long totalTicks = Convert.ToInt64(Convert.ToDouble(timeConsumed.Ticks) / (Convert.ToDouble(valOffset) / Convert.ToDouble(len)));
                            _totalTime = TimeSpan.FromTicks(totalTicks);
                            _timeRemain = _totalTime - timeConsumed;
                            this.OnChanged(EventArgs.Empty);
                        }
                    }
                }

            }
        }


        public event EventHandler Changed;

        protected virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }

        private TimeSpan _totalTime = TimeSpan.MaxValue;
        public TimeSpan TotalTime
        {
            get { return _totalTime; }
        }

        private TimeSpan _timeRemain = TimeSpan.MaxValue;
        public TimeSpan TimeRemain
        {
            get { return _timeRemain; }
        }


        public TimeSpan TimeCosumed
        {
            get { return DateTime.Now - _startTime; }
        }

        private bool _started = false;
        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
        }

        public virtual void Start()
        {
            this._startTime = DateTime.Now;
            _started = true;
        }


        #region IProgressMonitor Members


        public ProgressMonitorStyle Style
        {
            get;
            set;
        }

        #endregion
    }

    public class ParallelProgressMonitor : IProgressMonitor
    {
        private IProgressMonitor _owner;

        public ParallelProgressMonitor(IProgressMonitor progress, int count)
        {
            if (progress == null)
            {
                throw new ArgumentNullException("progress");
            }

            _owner = progress;

            if (count <= 0)
            {
                throw new ArgumentException(Resources.InvalidMultProgressStepCountText, "count");
            }

            decimal[] steps = new decimal[count];
            decimal len = (decimal)1 / (decimal)count;
            for (int i = 0; i < count; i++)
            {
                steps[i] = i < count - 1 ? len : 1 - len * (count - 1);
            }

            this.CreateSubProgresses(steps);
        }

        public ParallelProgressMonitor(IProgressMonitor progress, decimal[] steps)
        {
            if (progress == null)
            {
                throw new ArgumentNullException("progress");
            }

            _owner = progress;

            if (steps == null)
            {
                throw new ArgumentNullException("steps");
            }

            decimal sum = 0;

            foreach (decimal step in steps)
            {
                sum += step;
            }

            if (sum != 1)
            {
                throw new ArgumentException(Resources.InvalidMultProgressStepsText);
            }


            this.CreateSubProgresses(steps);


        }

        public IProgressMonitor this[int index]
        {
            get
            {
                return this._subProgresses[index];
            }
        }


        private SubProgress[] _subProgresses;

        private void CreateSubProgresses(decimal[] steps)
        {

            _subProgresses = new SubProgress[steps.Length];
            for (int i = 0; i < steps.Length; i++)
            {
                _subProgresses[i] = new SubProgress(this, steps[i]);
            }
        }



        public int Count
        {
            get
            {
                return _subProgresses.Length;
            }
        }



        private object _syncRoot = new object();

      

        public int Value
        {
            get
            {
                lock (_syncRoot)
                {
                    return _owner.Value;
                }
            }
            set
            {
                throw new InvalidOperationException(Resources.CannotSetValueOnMultProgress);
            }
        }

        private void SyncValue()
        {
            lock (_syncRoot)
            {
                decimal procentage = 0;
                foreach (SubProgress monitor in this._subProgresses)
                {
                    procentage += (decimal)(monitor.Value - monitor.Minimum) / (decimal)(monitor.Maximum - monitor.Minimum) * monitor.Length;
                }

                decimal value = this.Minimum + (this.Maximum - this.Minimum) * procentage;
                this._owner.Value = (int)Math.Round(value);
            }
        }

        public int Maximum
        {
            get
            {
                return this._owner.Maximum;
            }
            set
            {
                this._owner.Maximum = value;
            }
        }

        public int Minimum
        {
            get
            {
                return _owner.Minimum;
            }
            set
            {
                _owner.Minimum = value;
            }
        }
     
	
       
        private class SubProgress : IProgressMonitor
        {
            private decimal _length;

            public decimal Length
            {
                get
                {
                    return _length;
                }
            }

            private ParallelProgressMonitor _parent;

            public SubProgress(ParallelProgressMonitor parent, decimal length)
            {
                this._parent = parent;
                this._length = length;
            }


            private int _value = 0;

            public int Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    if (value < this.Minimum || value > this.Maximum)
                    {
                        throw new ArgumentOutOfRangeException("Value", value, Resources.InvalidProgressValueText);
                    }
                    if (_value != value)
                    {
                        this._value = value;
                        this._parent.SyncValue();
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
                    _maximum = value;
                    this._parent.SyncValue();
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
                    _minimum = value;
                    this._parent.SyncValue();
                }
            }



            public ProgressMonitorStyle Style
            {
                get { return this._parent.Style; }
                set {
                    this._parent.Style = value;}
            }

           
        }

        public ProgressMonitorStyle Style
        {
            get { return this._owner.Style; }
            set { this._owner.Style = value; }
        }



    }


    public class MemoryProgressMonitor : IProgressMonitor
    {
        #region IProgressMonitor Members

        private int _value = 0;

        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    this.OnValueChanged(EventArgs.Empty);
                }
            }
        }



        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged(EventArgs e)
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, e);
            }
        }

        private int _maximum = 100;

        public int Maximum
        {
            get { return _maximum; }
            set
            {
                if (_maximum != value)
                {
                    _maximum = value;
                    OnMaximumChanged(EventArgs.Empty);
                }
            }
        }


        public event EventHandler MaximumChanged;

        protected virtual void OnMaximumChanged(EventArgs e)
        {
            if (this.MaximumChanged != null)
            {
                this.MaximumChanged(this, e);
            }
        }




        private int _minimum = 0;

        public int Minimum
        {
            get { return _minimum; }
            set
            {
                if (_minimum != value)
                {
                    _minimum = value;
                    OnMinimumChanged(EventArgs.Empty);
                }
            }
        }


        public event EventHandler MinimumChanged;

        protected virtual void OnMinimumChanged(EventArgs e)
        {
            if (this.MinimumChanged != null)
            {
                this.MinimumChanged(this, e);
            }
        }

        private ProgressMonitorStyle _style = ProgressMonitorStyle.Blocks;

        public ProgressMonitorStyle Style
        {
            get { return _style; }
            set
            {
                if (_style != value)
                {
                    _style = value;
                    OnStyleChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler StyleChanged;

        protected virtual void OnStyleChanged(EventArgs e)
        {
            if (this.StyleChanged != null)
            {
                this.StyleChanged(this, e);
            }
        }

        #endregion
    }

}
