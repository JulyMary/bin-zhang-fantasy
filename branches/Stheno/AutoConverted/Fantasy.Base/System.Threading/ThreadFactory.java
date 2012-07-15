package System.Threading;

public class ThreadFactory
{
	public static Thread CreateThread(ParameterizedThreadStart start)
	{
		Thread rs = new Thread(start);
		InitThread(rs);
		return rs;
	}

	public static Thread CreateThread(ThreadStart start)
	{
		Thread rs = new Thread(start);
		InitThread(rs);
		return rs;
	}


	private static void InitThread(Thread thread)
	{
		thread.CurrentCulture = Thread.currentThread().CurrentCulture;
		thread.CurrentUICulture = Thread.currentThread().CurrentUICulture;
		thread.Priority = ThreadPriority.Lowest;
		thread.IsBackground = true;
	}

//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: public static bool QueueUserWorkItem(WaitCallback callBack, Object state = null)
	public static boolean QueueUserWorkItem(WaitCallback callBack, Object state)
	{
		CultureInfo ci = Thread.currentThread().CurrentCulture;
		CultureInfo uici = Thread.currentThread().CurrentUICulture;

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		return ThreadPool.QueueUserWorkItem((o) =>
		{
			Thread.currentThread().CurrentCulture = ci;
			Thread.currentThread().CurrentUICulture = ci;
			callBack(o);
		}
	   , state);

	}

}