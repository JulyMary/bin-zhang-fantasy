package fantasy.tracking;

import java.lang.ref.*;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;


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

	

}