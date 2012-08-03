package fantasy;

import java.util.concurrent.*;

public final class ThreadFactory {
    private  ThreadFactory()
    {
    	
    }
    
    
    private static ExecutorService _defaultPool = Executors.newCachedThreadPool();
    
    public static Thread create(Runnable runnable)
    {
    	Thread rs = new Thread(runnable);
    	rs.setDaemon(true);
    	rs.setPriority(Thread.MIN_PRIORITY);
    	
    	return rs;
    }
    
    public static Thread createAndStart(Runnable runnable)
    {
    	Thread rs = create(runnable);
    	rs.start();
    	
    	return rs;
    }
    
    public static void queueUserWorkItem(Runnable runnable)
    {
    	_defaultPool.execute(runnable);
    }
    
    
}
