package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;
import Fantasy.Properties.*;

public class ProgressTimeEvaluator implements IProgressMonitor
{

	private int _maximum = 100;
	public final int getMaximum()
	{
		return _maximum;
	}
	public final void setMaximum(int value)
	{
		_maximum = value;
	}

	private int _minimum = 0;
	public final int getMinimum()
	{
		return _minimum;
	}
	public final void setMinimum(int value)
	{
		_minimum = value;
	}
	private int _value;
	public final int getValue()
	{
		return _value;
	}
	public final void setValue(int value)
	{
		if ((this._value != value))
		{
			_value = value;
			if (this._started)
			{
				int valOffset = _value - _minimum;
				int len = this._maximum - this._minimum;
				TimeSpan timeConsumed = this.getTimeCosumed();
				if (valOffset > 0 && len > 0 && timeConsumed != TimeSpan.Zero)
				{
					long totalTicks = Long.parseLong(Double.parseDouble(timeConsumed.Ticks) / (Double.parseDouble(valOffset) / Double.parseDouble(len)));
					_totalTime = TimeSpan.FromTicks(totalTicks);
					_timeRemain = _totalTime - timeConsumed;
					this.OnChanged(EventArgs.Empty);
				}
			}
		}

	}


//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Changed;

	protected void OnChanged(EventArgs e)
	{
		if (Changed != null)
		{
			Changed(this, e);
		}
	}

	private TimeSpan _totalTime = TimeSpan.getMaxValue();
	public final TimeSpan getTotalTime()
	{
		return _totalTime;
	}

	private TimeSpan _timeRemain = TimeSpan.getMaxValue();
	public final TimeSpan getTimeRemain()
	{
		return _timeRemain;
	}


	public final TimeSpan getTimeCosumed()
	{
		return new java.util.Date() - _startTime;
	}

	private boolean _started = false;
	private java.util.Date _startTime = new java.util.Date(0);
	public final java.util.Date getStartTime()
	{
		return _startTime;
	}

	public void Start()
	{
		this._startTime = new java.util.Date();
		_started = true;
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IProgressMonitor Members


	private ProgressMonitorStyle privateStyle = ProgressMonitorStyle.forValue(0);
	public final ProgressMonitorStyle getStyle()
	{
		return privateStyle;
	}
	public final void setStyle(ProgressMonitorStyle value)
	{
		privateStyle = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}