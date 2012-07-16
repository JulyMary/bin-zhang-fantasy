package fantasy.tracking;

import fantasy.*;
import java.lang.ref.*;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;


final class RefreshManager implements Runnable
{

	private  java.util.ArrayList<WeakReference<IRefreshable>> _list = new java.util.ArrayList<WeakReference<IRefreshable>>();
	private static RefreshManager _instance;
	static
	{

		_instance = new RefreshManager();
		_refreshThread = ThreadFactory.createAndStart(_instance);

	}



	@Override
	public  void run()
	{

		do
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
						((IRefreshable)weak.get()).Refresh();
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

			try {
				Thread.sleep(5 * 1000);
			} catch (InterruptedException e) {
				return;
			}

		} while (true);
	}

	@SuppressWarnings("unused")
	private static Thread _refreshThread;

	public static void Register(IRefreshable obj)
	{
		synchronized (RefreshManager.class)
		{
			_instance._list.add(new WeakReference<IRefreshable>(obj));
		}
	}

	public static void Unregister(IRefreshable obj)
	{
		synchronized (RefreshManager.class)
		{
			int index = -1;
			for(int i = 0; i < _instance._list.size(); i ++)
			{
				if(_instance._list.get(i).get() == obj)
				{
					index = i;
					break;
				}

			}

			if(index > 0)
			{
				_instance._list.remove(index);
			}

		}
	}


}