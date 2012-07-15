package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;
import Fantasy.Properties.*;

public class MemoryProgressMonitor implements IProgressMonitor
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IProgressMonitor Members

	private int _value = 0;

	public final int getValue()
	{
		return _value;
	}
	public final void setValue(int value)
	{
		if (_value != value)
		{
			_value = value;
			this.OnValueChanged(EventArgs.Empty);
		}
	}



//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler ValueChanged;

	protected void OnValueChanged(EventArgs e)
	{
		if (this.ValueChanged != null)
		{
			this.ValueChanged(this, e);
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
			_maximum = value;
			OnMaximumChanged(EventArgs.Empty);
		}
	}


//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler MaximumChanged;

	protected void OnMaximumChanged(EventArgs e)
	{
		if (this.MaximumChanged != null)
		{
			this.MaximumChanged(this, e);
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
			OnMinimumChanged(EventArgs.Empty);
		}
	}


//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler MinimumChanged;

	protected void OnMinimumChanged(EventArgs e)
	{
		if (this.MinimumChanged != null)
		{
			this.MinimumChanged(this, e);
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
			OnStyleChanged(EventArgs.Empty);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler StyleChanged;

	protected void OnStyleChanged(EventArgs e)
	{
		if (this.StyleChanged != null)
		{
			this.StyleChanged(this, e);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}