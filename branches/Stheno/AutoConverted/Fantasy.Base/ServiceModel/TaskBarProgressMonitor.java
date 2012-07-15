package Fantasy.ServiceModel;

import Microsoft.WindowsAPICodePack.Taskbar.*;

public class TaskBarProgressMonitor implements IProgressMonitor
{

	private TaskbarProgressBarState _state = TaskbarProgressBarState.Normal;
	private boolean _supported = TaskbarManager.getIsPlatformSupported();
	private int _value = 0;
	public final int getValue()
	{
		return _value;
	}
	public final void setValue(int value)
	{
		if (_value != value)
		{
			this._value = value;
			this.SetTaskBar();
		}
	}

	private void SetTaskBar()
	{
		if (_supported)
		{
			TaskbarProgressBarState newState;
			if (this.getStyle() == ProgressMonitorStyle.Blocks)
			{
				if (this.getValue() == this.getMinimum())
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
				TaskbarManager.getInstance().SetProgressState(_state);
			}
			if (_state == TaskbarProgressBarState.Normal)
			{
				TaskbarManager.getInstance().SetProgressValue(this.getValue() - this.getMinimum(), this.getMaximum() - this.getMinimum());
			}
		}
	}


	private int _maximum = 100;
	public final int getMaximum()
	{
		return _maximum;
	}
	public final void setMaximum(int value)
	{
		if (_maximum != value)
		{
			this._maximum = value;
			this.SetTaskBar();
		}
	}

	private int _minimum = 0;
	public final int getMinimum()
	{
		return _minimum;
	}
	public final void setMinimum(int value)
	{
		if (_minimum != value)
		{
			_minimum = value;
			this.SetTaskBar();
		}
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
			this.SetTaskBar();
		}
	}


}