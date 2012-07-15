package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;
import Fantasy.Properties.*;

public class ProgressMonitorContainer implements IProgressMonitorContainer
{

	private java.util.ArrayList _monitors = new java.util.ArrayList();
	private int _value = 0;
	private int _maximum = 100;
	private int _minimum = 0;

	private int _step = 1;
	public final void Increment(int Value)
	{
		this.setValue(this.getValue() + Value);
	}

	public final int getMaximum()
	{
		return this._maximum;
	}
	public final void setMaximum(int value)
	{
		this._maximum = value;
		for (IProgressMonitor monitor : this._monitors)
		{
			monitor.setMaximum(value);
		}
	}

	public final int getMinimum()
	{
		return this._minimum;
	}
	public final void setMinimum(int value)
	{
		this._minimum = value;
		for (IProgressMonitor monitor : this._monitors)
		{
			monitor.setMinimum(value);
		}
	}

	public final int getValue()
	{
		return this._value;
	}
	public final void setValue(int value)
	{
		if (value < this._minimum)
		{
			this._value = this._minimum;
		}
		else if (value > this.getMaximum())
		{
			this._value = this.getMaximum();
		}
		else
		{
			this._value = value;
		}

		for (IProgressMonitor monitor : this._monitors)
		{
			monitor.setValue(this._value);
		}

	}

	public final void AddMoniter(IProgressMonitor monitor)
	{
		monitor.setMaximum(this.getMaximum());
		monitor.setMinimum(this.getMinimum());
		monitor.setValue(this.getValue());
		monitor.setStyle(this.getStyle());
		this._monitors.add(monitor);
	}

	public final void RemoveMoniter(IProgressMonitor monitor)
	{
		this._monitors.remove(monitor);
	}

	public final void PerformStep()
	{
		this.Increment(this._step);
	}

	public final int getStep()
	{
		return this._step;
	}
	public final void setStep(int value)
	{
		this._step = value;
	}

	private ProgressMonitorStyle _style = ProgressMonitorStyle.Blocks;

	public final ProgressMonitorStyle getStyle()
	{
		return _style;
	}
	public final void setStyle(ProgressMonitorStyle value)
	{
		if (_style != value)
		{
			_style = value;

			for (IProgressMonitor monitor : this._monitors)
			{
				monitor.setStyle(_style);
			}
		}
	}


}