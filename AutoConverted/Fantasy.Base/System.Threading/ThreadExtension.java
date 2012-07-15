package System.Threading;

public final class ThreadExtension
{
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static Thread WithStart(this Thread thread)
	public static Thread WithStart(Thread thread)
	{
		thread.start();
		return thread;
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static Thread WithStart(this Thread thread, object state)
	public static Thread WithStart(Thread thread, Object state)
	{
		thread.Start(state);
		return thread;
	}
}