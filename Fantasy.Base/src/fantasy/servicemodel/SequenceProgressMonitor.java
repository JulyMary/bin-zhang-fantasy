package fantasy.servicemodel;

import java.math.*;

import java.util.*;

public class SequenceProgressMonitor implements IProgressMonitor, Iterable<IProgressMonitor>
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
			this.Length = length;
		}

		public final int getMaximum()
		{
			return this._maximum;
		}
		public final void setMaximum(int value)
		{
			this._maximum = value;
		}

		
		
		
		@Override
		public final int getMinimum()
		{
			return this._minimum;
		}
		@Override
		public final void setMinimum(int value)
		{
			this._minimum = value;
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
		this._children =  new SubProgressMonitor[subLengths.length];
		for (int i = 0; i <= subLengths.length - 1; i++)
		{
			SubProgressMonitor subMonitor = new SubProgressMonitor(this, start, subLengths[i]);
			this._children[i] = subMonitor;
			start += subLengths[i];
		}
		if (start != 1.0)
		{
			throw new IllegalArgumentException("subLengths");
		}
	}

	public SequenceProgressMonitor(IProgressMonitor monitor, int count)
	{
		double[] subLengths = new double[count];

		
		BigDecimal bd = new BigDecimal(1d/ (double)count);
		bd.setScale(4, RoundingMode.CEILING);
		double avg = bd.doubleValue();
		
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
		double percentage = (monitor.getValue() - monitor.getMinimum()) / (monitor.getMaximum() - monitor.getMinimum());
		double value = this.getMinimum() + (this.getMaximum() - this.getMinimum()) * monitor.StartPoint;
		value += (this.getMaximum() - this.getMinimum()) * monitor.Length * percentage;
		this._owner.setValue((int)Math.round(value));
	}


	@Override
	public Iterator<IProgressMonitor> iterator() {
		ArrayList<IProgressMonitor> rs = new ArrayList<IProgressMonitor>(this._children.length);
		for(int i = 0; i < this._children.length; i ++)
		{
			rs.add(this._children[i]);
			
		}
		return rs.iterator();
		
	}
}