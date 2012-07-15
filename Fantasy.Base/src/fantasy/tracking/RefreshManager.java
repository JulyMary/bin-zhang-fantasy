package fantasy.tracking;

public final class RefreshManager
{

	private static java.util.ArrayList<WeakReference> _list = new java.util.ArrayList<WeakReference>();

	static
	{
		_refreshThread = ThreadFactory.CreateThread(new ThreadStart(Run)).WithStart();


	}

	private static void Run()
	{
		do
		{
			java.util.ArrayList<WeakReference> list;
			synchronized (RefreshManager.class)
			{
				list = new java.util.ArrayList<WeakReference>(_list);
			}

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			Parallel.ForEach(list, weak =>
			{
				if (weak.IsAlive)
				{
					((IRefreshable)weak.Target).Refresh();
				}
				else
				{
					synchronized (_list)
					{
						_list.remove(weak);
					}
				}
			}
		   );

			Thread.sleep(5 * 1000);

		} while (true);
	}

	private static Thread _refreshThread;

	public static void Register(IRefreshable obj)
	{
		synchronized (RefreshManager.class)
		{
			_list.add(new WeakReference(obj));
		}
	}

	public static void Unregister(IRefreshable obj)
	{
		synchronized (RefreshManager.class)
		{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
			WeakReference weak = (from w in _list where w.Target == obj select w).SingleOrDefault();
			if (weak != null)
			{
				_list.remove(weak);
			}
		}
	}

}