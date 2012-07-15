package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;
import Fantasy.Properties.*;

public class ProgressBarMonitor implements IProgressMonitor
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
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
			this._control.HandleCreated += new EventHandler(Control_HandleCreated);
		}
	}

	private void Control_HandleCreated(Object sender, EventArgs e)
	{
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		this._control.HandleCreated -= new EventHandler(Control_HandleCreated);
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		this._control.invoke(new MethodInvoker(() =>
		{
			switch (this.getStyle())
			{
				case Blocks:
					this._control.Style = ProgressBarStyle.Blocks;
					break;
					case Continuous:
						this._control.Style = ProgressBarStyle.Continuous;
						break;
						case Marquee:
							this._control.Style = ProgressBarStyle.Marquee;
							break;
			}
			this._control.Minimum = this.getMinimum();
			this._control.Maximum = this.getMaximum();
			this._control.setValue(this.getValue());
		}
	   ));


	}

	private int _maximum = 100;
	public final int getMaximum()
	{
		return _maximum;
	}

	public final void setMaximum(int value)
	{
		this._maximum = value;
		if (this._control.IsHandleCreated)
		{

			if ((this._control.InvokeRequired))
			{
				MethodInvoker<String, Integer> del = new MethodInvoker<String, Integer>(SetProperty);
				this._control.invoke(del, "maximum", value);
			}
			else
			{
				this._control.Maximum = value;
			}
		}

	}

	private void SetProperty(String name, int value)
	{
//C# TO JAVA CONVERTER NOTE: The following 'switch' operated on a string member and was converted to Java 'if-else' logic:
//		switch (name)
//ORIGINAL LINE: case "maximum":
		if (name.equals("maximum"))
		{
				this._control.Maximum = value;
		}
//ORIGINAL LINE: case "minimum":
		else if (name.equals("minimum"))
		{
				this._control.Minimum = value;
		}
//ORIGINAL LINE: case "value":
		else if (name.equals("value"))
		{
				this._control.setValue(value);
		}

	}

	private int _minimum = 0;
	public final int getMinimum()
	{
		return _minimum;
	}
	public final void setMinimum(int value)
	{
		this._minimum = value;
		if (this._control.IsHandleCreated)
		{
			if ((this._control.InvokeRequired))
			{
				MethodInvoker<String, Integer> del = new MethodInvoker<String, Integer>(SetProperty);
				this._control.invoke(del, "minimum", value);
			}
			else
			{
				this._control.Minimum = value;
			}
		}

	}

	private int _value;

	public final int getValue()
	{
		return this._value;
	}
	public final void setValue(int value)
	{
		if (this._control.IsHandleCreated)
		{
			this._value = value;
			if ((this._control.InvokeRequired))
			{
				MethodInvoker<String, Integer> del = new MethodInvoker<String, Integer>(SetProperty);
				this._control.invoke(del, "value", value);
			}
			else
			{
				this._control.setValue(value);
			}
		}
	}

	private ProgressMonitorStyle _style = ProgressMonitorStyle.Blocks;
	public final ProgressMonitorStyle getStyle()
	{

		return this._style;
	}
	public final void setStyle(ProgressMonitorStyle value)
	{
		this._style = value;
		if (this._control.IsHandleCreated)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			this._control.invoke(new MethodInvoker(() =>
			{
				switch (value)
				{
					case Blocks:
						this._control.Style = ProgressBarStyle.Blocks;
						break;
						case Continuous:
							this._control.Style = ProgressBarStyle.Continuous;
							break;
							case Marquee:
								this._control.Style = ProgressBarStyle.Marquee;
								break;
				}
			}
		   ));
		}
	}
}