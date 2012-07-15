package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;
import Fantasy.Properties.*;

public class SequenceProgressMonitor implements IProgressMonitor, Iterable
{

	private IProgressMonitor _owner;

	private SubProgressMonitor[] _children;

	private static class SubProgressMonitor implements IProgressMonitor
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
			this.setLength(length);
		}

		public final int getMaximum()
		{
			return this._maximum;
		}
		public final void setMaximum(int value)
		{
			this._maximum = value;
		}

		public final int getMinimun()
		{
			return this._minimum;
		}
		public final void setMinimun(int value)
		{
			this._minimum = value;
		}
		private int getMinimum()
		{
			return getMinimun();
		}
		private void setMinimum(int value)
		{
			setMinimun(value);
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
			this._owner.OnSubMonitorValueChange(this);
		}

		public final ProgressMonitorStyle getStyle()
		{
			return this._owner.getStyle();
		}
		public final void setStyle(ProgressMonitorStyle value)
		{
			this._owner.setStyle(value);
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
		this._children = (SubProgressMonitor[])Array.CreateInstance(SubProgressMonitor.class, subLengths.length);
		for (int i = 0; i <= subLengths.length - 1; i++)
		{
			SubProgressMonitor subMonitor = new SubProgressMonitor(this, start, subLengths[i]);
			this._children[i] = subMonitor;
			start += subLengths[i];
		}
		if (start != 1.0)
		{
			throw new ApplicationException(Resources.getInvalidMultProgressStepsText());
		}
	}

	public SequenceProgressMonitor(IProgressMonitor monitor, int count)
	{
		double[] subLengths = (double[])Array.CreateInstance(Double.class, count);
//C# TO JAVA CONVERTER TODO TASK: Math.Round can only be converted to Java's Math.round if just one argument is used:
		double avg = Math.Round(1.0 / Double.parseDouble(count), 4);
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

	public final IProgressMonitor getItem(int index)
	{
		return _children[index];
	}

	public final int getCount()
	{
		return _children.length;
	}

	public final int getMaximum()
	{
		return this._owner.getMaximum();
	}
	public final void setMaximum(int value)
	{
		this._owner.setMaximum(value);
	}

	public final int getMinimum()
	{
		return this._owner.getMinimum();
	}
	public final void setMinimum(int value)
	{
		this._owner.setMinimum(value);
	}

	public final int getValue()
	{
		return this._owner.getValue();
	}
	public final void setValue(int value)
	{
		this._owner.setValue(value);
	}

	public final ProgressMonitorStyle getStyle()
	{
		return this._owner.getStyle();
	}
	public final void setStyle(ProgressMonitorStyle value)
	{
		this._owner.setStyle(value);
	}

	private void OnSubMonitorValueChange(SubProgressMonitor monitor)
	{
		double percentage = (monitor.getValue() - monitor.getMinimun()) / (monitor.getMaximum() - monitor.getMinimun());
		double value = this.getMinimum() + (this.getMaximum() - this.getMinimum()) * monitor.StartPoint;
		value += (this.getMaximum() - this.getMinimum()) * monitor.getLength() * percentage;
		this._owner.setValue((int)Math.round(value));
	}

	public final java.util.Iterator GetEnumerator()
	{
		return this._children.iterator();
	}
}