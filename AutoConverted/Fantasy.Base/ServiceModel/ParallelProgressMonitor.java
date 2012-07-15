package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;
import Fantasy.Properties.*;

public class ParallelProgressMonitor implements IProgressMonitor
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
			throw new IllegalArgumentException(Resources.getInvalidMultProgressStepCountText(), "count");
		}

		java.math.BigDecimal[] steps = new java.math.BigDecimal[count];
		java.math.BigDecimal len = (java.math.BigDecimal)1 / (java.math.BigDecimal)count;
		for (int i = 0; i < count; i++)
		{
			steps[i] = new java.math.BigDecimal(i) < count - 1 ? len : 1 - len.multiply((count - 1));
		}

		this.CreateSubProgresses(steps);
	}

	public ParallelProgressMonitor(IProgressMonitor progress, java.math.BigDecimal[] steps)
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

		java.math.BigDecimal sum = new java.math.BigDecimal(0);

		for (java.math.BigDecimal step : steps)
		{
			sum += step;
		}

		if (!sum.equals(1))
		{
			throw new IllegalArgumentException(Resources.getInvalidMultProgressStepsText());
		}


		this.CreateSubProgresses(steps);


	}

	public final IProgressMonitor getItem(int index)
	{
		return this._subProgresses[index];
	}


	private SubProgress[] _subProgresses;

	private void CreateSubProgresses(java.math.BigDecimal[] steps)
	{

		_subProgresses = new SubProgress[steps.length];
		for (int i = 0; i < steps.length; i++)
		{
			_subProgresses[i] = new SubProgress(this, steps[i]);
		}
	}



	public final int getCount()
	{
		return _subProgresses.getLength();
	}



	private Object _syncRoot = new Object();



	public final int getValue()
	{
		synchronized (_syncRoot)
		{
			return _owner.getValue();
		}
	}
	public final void setValue(int value)
	{
		throw new InvalidOperationException(Resources.getCannotSetValueOnMultProgress());
	}

	private void SyncValue()
	{
		synchronized (_syncRoot)
		{
			java.math.BigDecimal procentage = new java.math.BigDecimal(0);
			for (SubProgress monitor : this._subProgresses)
			{
				procentage += (java.math.BigDecimal)(monitor.getValue() - monitor.getMinimum()) / (java.math.BigDecimal)(monitor.getMaximum() - monitor.getMinimum()) * monitor.getLength();
			}

			java.math.BigDecimal value = new java.math.BigDecimal(this.getMinimum()) + (this.getMaximum() - this.getMinimum()) * procentage;
			this._owner.setValue((int)Math.round(value));
		}
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
		return _owner.getMinimum();
	}
	public final void setMinimum(int value)
	{
		_owner.setMinimum(value);
	}



	private static class SubProgress implements IProgressMonitor
	{
		private java.math.BigDecimal _length = new java.math.BigDecimal(0);

		public final java.math.BigDecimal getLength()
		{
			return _length;
		}

		private ParallelProgressMonitor _parent;

		public SubProgress(ParallelProgressMonitor parent, java.math.BigDecimal length)
		{
			this._parent = parent;
			this._length = length;
		}


		private int _value = 0;

		public final int getValue()
		{
			return _value;
		}
		public final void setValue(int value)
		{
			if (value < this.getMinimum() || value > this.getMaximum())
			{
				throw new ArgumentOutOfRangeException("Value", value, Resources.getInvalidProgressValueText());
			}
			if (_value != value)
			{
				this._value = value;
				this._parent.SyncValue();
			}
		}

		private int _maximum = 100;
		public final int getMaximum()
		{
			return _maximum;
		}
		public final void setMaximum(int value)
		{
			_maximum = value;
			this._parent.SyncValue();
		}


		private int _minimum = 0;

		public final int getMinimum()
		{
			return _minimum;
		}
		public final void setMinimum(int value)
		{
			_minimum = value;
			this._parent.SyncValue();
		}



		public final ProgressMonitorStyle getStyle()
		{
			return this._parent.getStyle();
		}
		public final void setStyle(ProgressMonitorStyle value)
		{
			this._parent.setStyle(value);
		}


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