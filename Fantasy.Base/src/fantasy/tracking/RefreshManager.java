package fantasy.tracking;

import java.lang.ref.*;


import fantasy.collections.*;
import java.util.concurrent.*;

final class RefreshManager 
{

	private static  java.util.ArrayList<WeakReference<IRefreshable>> _list = new java.util.ArrayList<WeakReference<IRefreshable>>();
	private static ScheduledExecutorService _scheduler;
	static
	{

		_scheduler = Executors.newScheduledThreadPool(1);
		_scheduler.scheduleAtFixedRate(new Runnable(){

			@Override
			public void run() {
				refresh();
			}}, 5, 5, TimeUnit.SECONDS);
		_scheduler.shutdown();
		

	}
	
    private RefreshManager()
    {
    	
    }
		   
	private static  void refresh()
	{

		java.util.ArrayList<WeakReference<IRefreshable>> list;
		synchronized (RefreshManager.class)
		{
			list = new java.util.ArrayList<WeakReference<IRefreshable>>(_list);
		}


		ExecutorService exec = Executors.newCachedThreadPool();
		try
		{
			for(WeakReference<IRefreshable> weak : list)
			{
				if(!weak.isEnqueued())
				{
					try
					{
						final IRefreshable item = weak.get();
					    exec.execute(new Runnable(){

							@Override
							public void run() {
								
							   item.refresh();
							}});
					}
					catch(Exception error)
					{
						synchronized (RefreshManager.class)
						{
							_list.remove(weak);
						}
					}
				}
				else
				{
					synchronized (RefreshManager.class)
					{
						_list.remove(weak);
					}
				}
			}
		}
		finally
		{
			exec.shutdown();
		}

			
		
	}

	

	public static void register(IRefreshable obj)
	{
		synchronized (RefreshManager.class)
		{
			_list.add(new WeakReference<IRefreshable>(obj));
		}
	}
	
	public static void unregister(final IRefreshable obj)
	{
		synchronized (RefreshManager.class)
		{
			try
			{
				WeakReference<IRefreshable> ref = new Enumerable<WeakReference<IRefreshable>>(_list).firstOrDefault(new Predicate<WeakReference<IRefreshable>>(){

					@Override
					public boolean evaluate(WeakReference<IRefreshable> ref) {
						return ref.get() == obj;
					}});
				if(ref != null)
				{
					_list.remove(ref);
				}
			}
			catch(Exception error)
			{
				
			}
			
		}
	}

	

}