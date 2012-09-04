package fantasy.jobs.solar;

import java.util.*;

import fantasy.*;
import fantasy.collections.*;
import fantasy.jobs.management.*;

public class JobStartupSatelliteFilter extends ObjectWithSite implements IJobStartupFilter
{


	private double getLoadFactor(SatelliteManager manager, String name)
	{
		double rs = 0;
		RefObject<Double> tempRef_rs = new RefObject<Double>(rs);

		boolean tempVar = manager.SafeCallSatellite(name, new Func1<ISatellite, Double>(){

			@Override
			public Double call(ISatellite satellite) throws Exception {
				return satellite.getLoadFactor();
			}},  tempRef_rs);
			rs = tempRef_rs.argvalue;
		if (tempVar)
		{
			return rs;
		}
		else
		{
			return 0;
		}
	}

	public final Iterable<JobStartupData> filter(Iterable<JobStartupData> source) throws Exception
	{
		SatelliteManager manager = this.getSite().getRequiredService(SatelliteManager.class);
		
		final Hashtable<String, Double> factors = new Hashtable<String, Double>();

		for(SatelliteSite satellite : manager.getSatellites())
		{
			factors.put(satellite.getName(), this.getLoadFactor(manager, satellite.getName()));
		}
		
		Enumerable<String> names = new Enumerable<SatelliteSite>(manager.getSatellites()).select(new Selector<SatelliteSite, String>(){

			@Override
			public String select(SatelliteSite item) {
				return item.getName();
			}}).orderByDescending(new Selector<String, Double>(){

				@Override
				public Double select(String item) {
					Double rs = factors.get(item);
					return rs != null ? rs : 0;
				}});


		return new CrossJoinIterable(source, names);

	}

	private class CrossJoinIterable implements Iterable<JobStartupData>
	{
		public CrossJoinIterable(Iterable<JobStartupData> source, Iterable<String> names)
		{
			this._source = source;
			this._names = names;
		}
		
		private Iterable<JobStartupData> _source;
		private Iterable<String> _names;
		@Override
		public Iterator<JobStartupData> iterator() {
			return new CrossJoinIterator(_source, _names);
		}
	}
	
	private class CrossJoinIterator implements Iterator<JobStartupData>
	{
        public CrossJoinIterator(Iterable<JobStartupData> source, Iterable<String> names)
        {
        	this._source = source.iterator();
        	this._names = new ArrayList<String>();
        	for(String name : names)
        	{
        		_names.add(name);
        	}
        }
		
		private Iterator<JobStartupData> _source;
		private ArrayList<String> _names;
		private int _index = -1;
		private JobMetaData _job = null;
		
		@Override
		public boolean hasNext() {
			
			if(this._index == -1)
			{
				return  _names.size() > 0 && _source.hasNext();
			}
			else
			{
				return _index < _names.size() || _source.hasNext();
			}
			
			
		}

		@Override
		public JobStartupData next() {
			
			if(this.hasNext())
			{
				if(_index == -1)
				{
					_index = 0;
				}
				
				if(_index > _names.size() - 1 )
				{
					_job = _source.next().getJobMetaData();
					this._index = 0;
				}
				
				JobStartupData rs = new JobStartupData();
				rs.setJobMetaData(_job);
				rs.setSatellite(_names.get(this._index));
				
				this._index ++;
				return rs;
				
			}
			else
			{
				throw new NoSuchElementException();
			}
			
		}

		@Override
		public void remove() {
			throw new UnsupportedOperationException();
			
		}
		
	}
	
}