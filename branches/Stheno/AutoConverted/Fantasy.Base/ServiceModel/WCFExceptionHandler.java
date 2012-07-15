package Fantasy.ServiceModel;

public final class WCFExceptionHandler
{

	private static java.lang.Class[] _knowns = new java.lang.Class[] { EndpointNotFoundException.class, TimeoutException.class, CommunicationException.class, ServerTooBusyException.class, CommunicationObjectAbortedException.class, CommunicationObjectFaultedException.class, ObjectDisposedException.class, CallbackExpiredException.class };

	public static void CatchKnowns(RuntimeException error)
	{
		java.lang.Class et = error.getClass();

		if (et.IsGenericType && et.GetGenericTypeDefinition() == FaultException<>.class)
		{
			et = et.GetGenericArguments()[0];
		}


		if (Array.indexOf(_knowns, et) < 0)
		{
			throw error;
		}
	}

	public static boolean CanCatch(RuntimeException error)
	{
		java.lang.Class et = error.getClass();

		if (et.IsGenericType && et.GetGenericTypeDefinition() == FaultException<>.class)
		{
			et = et.GetGenericArguments()[0];
		}

		return Array.indexOf(_knowns, et) >= 0;

	}
}