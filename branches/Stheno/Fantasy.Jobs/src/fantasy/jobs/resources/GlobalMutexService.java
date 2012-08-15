package fantasy.jobs.resources;

import java.rmi.RemoteException;
import java.util.*;

import org.apache.commons.lang3.StringUtils;
import fantasy.*;
import fantasy.collections.*;
import fantasy.jobs.management.*;
import fantasy.jobs.scheduling.*;
import fantasy.rmi.RmiUtils;
import fantasy.servicemodel.*;

public class GlobalMutexService extends AbstractService implements IGlobalMutexService
{

	/**
	 * 
	 */
	private static final long serialVersionUID = -5467134033299170728L;

	public GlobalMutexService() throws RemoteException {
		super();
	}

	private java.util.ArrayList<Mutex> _allocated = new java.util.ArrayList<Mutex>();
	private Object _syncRoot = new Object();
	private IScheduleService _scheduleService;
	private IJobDispatcher _jobDispatcher;

	@Override
	public void initializeService() throws Exception
	{
		_scheduleService = this.getSite().getRequiredService(IScheduleService.class);
		_jobDispatcher = this.getSite().getRequiredService(IJobDispatcher.class);
		RmiUtils.bind(this);
		super.initializeService();
	}
	
	@Override
	public void uninitializeService() throws Exception
	{
		super.uninitializeService();
		RmiUtils.unbind(this);
	}

	private static class Mutex
	{
		private String privateName;
		public final String getName()
		{
			return privateName;
		}
		public final void setName(String value)
		{
			privateName = value;
		}

		private long privateCookie;
		public final long getCookie()
		{
			return privateCookie;
		}
		public final void setCookie(long value)
		{
			privateCookie = value;
		}
	}


	public final boolean isAvaiable(final String key)
	{
		synchronized (_syncRoot)
		{
			return ! new Enumerable<Mutex>(this._allocated).any(new Predicate<Mutex>(){

				@Override
				public boolean evaluate(Mutex m) throws Exception {
					return StringUtils.equalsIgnoreCase(m.getName(), key);
				}});
		}
	}
	
	

	public final boolean request(final String key, TimeSpan timeout) throws Exception
	{
		synchronized (_syncRoot)
		{
			Mutex mutex = new Enumerable<Mutex>(this._allocated).firstOrDefault(new Predicate<Mutex>(){

				@Override
				public boolean evaluate(Mutex m) throws Exception {
					return StringUtils.equalsIgnoreCase(m.getName(), key);
				}});
			if (mutex == null)
			{
				final Mutex newMutex = new Mutex();
				newMutex.setName(key);
				
				newMutex.setCookie(_scheduleService.register(timeout.add(new Date()), new Action (){

					@Override
					public void act() throws Exception {
						GlobalMutexService.this.release(newMutex);
						
					}})); 
				this._allocated.add(newMutex);
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	private void release(Mutex mutex)
	{
		synchronized (_syncRoot)
		{
			_allocated.remove(mutex);
		}
		if (_jobDispatcher != null)
		{
			_jobDispatcher.TryDispatch();
		}

	}

	public final void release(final String key) throws Exception
	{
		synchronized (_syncRoot)
		{
			Mutex mutex = new Enumerable<Mutex>(this._allocated).firstOrDefault(new Predicate<Mutex>(){

				@Override
				public boolean evaluate(Mutex m) throws Exception {
					return StringUtils.equalsIgnoreCase(m.getName(), key);
				}});
			if (mutex != null)
			{
				this.release(mutex);
				_scheduleService.unregister(mutex.getCookie());
			}
		}
	}

}