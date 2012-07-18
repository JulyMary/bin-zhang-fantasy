package fantasy.servicemodel;




public class ParallelProgressMonitor implements IProgressMonitor
{
	private IProgressMonitor _owner;

	public ParallelProgressMonitor(IProgressMonitor progress, int count)
	{
		if (progress == null)
		{
			throw new IllegalArgumentException("progress");
		}

		_owner = progress;

		if (count <= 0)
		{
			throw new IllegalArgumentException("count");
		}

		double[] steps = new double[count];
		double len = 1d / (double)count;
		for (int i = 0; i < count; i++)
		{
			steps[i] = i < count - 1 ? len : 1 - len * ((count - 1));
		}

		this.CreateSubProgresses(steps);
	}

	public ParallelProgressMonitor(IProgressMonitor progress, double[] steps)
	{
		if (progress == null)
		{
			throw new IllegalArgumentException("progress");
		}

		_owner = progress;

		if (steps == null)
		{
			throw new IllegalArgumentException("steps");
		}

		double sum = 0;

		for (double step : steps)
		{
			sum += step;
		}

		if (sum != 1)
		{
			throw new IllegalArgumentException("steps");
		}


		this.CreateSubProgresses(steps);


	}

	public final IProgressMonitor getItem(int index)
	{
		return this._subProgresses[index];
	}


	private SubProgress[] _subProgresses;

	private void CreateSubProgresses(double[] steps)
	{

		_subProgresses = new SubProgress[steps.length];
		for (int i = 0; i < steps.length; i++)
		{
			_subProgresses[i] = new SubProgress(this, steps[i]);
		}
	}



	public final int getCount()
	{
		return _subProgresses.length;
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
		throw new IllegalStateException("Cannot set value on ParallelProgressMonitor");
	}

	private void SyncValue()
	{
		synchronized (_syncRoot)
		{
			double procentage = 0;
			for (SubProgress monitor : this._subProgresses)
			{
				procentage += (double)(monitor.getValue() - monitor.getMinimum()) / (double)(monitor.getMaximum() - monitor.getMinimum()) * monitor.getLength();
			}

			double value = (this.getMinimum()) + (this.getMaximum() - this.getMinimum()) * procentage;
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
		private double _length = 0;

		public final double getLength()
		{
			return _length;
		}

		private ParallelProgressMonitor _parent;

		public SubProgress(ParallelProgressMonitor parent, double length)
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
				throw new IllegalArgumentException("Value");
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