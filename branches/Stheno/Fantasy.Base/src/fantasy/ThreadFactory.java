package fantasy;

public final class ThreadFactory {
    private  ThreadFactory()
    {
    	
    }
    
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
    
    
}
