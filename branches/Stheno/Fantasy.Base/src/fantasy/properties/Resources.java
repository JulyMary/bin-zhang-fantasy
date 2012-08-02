package fantasy.properties;

public final class Resources {

	private Resources()
	{
		
	}
	
	public static String getSystemCategory()
	{
		return "System";
	}
	
	public static String getServicesCategory()
	{
		return "Services";
	}
	
	public static String getUnhandledExceptionMessage()
	{
		return "An unhandled exception is throwed by current JVM.";
	}

	public static String getServiceFailedInitializeMessage() {
		return "Service %1$s failed to initialize.";
	}
	
	public static String getServiceInitializedMessage()
	{
		return "Service %1$s initialized.";
	}

	public static String getServiceUninitializedMessage() {
		
		return "Service %1$s uninitialized";
	}
	
}
