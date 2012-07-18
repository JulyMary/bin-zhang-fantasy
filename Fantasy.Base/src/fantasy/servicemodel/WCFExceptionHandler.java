package fantasy.servicemodel;

import java.rmi.*;

import org.apache.commons.lang3.ArrayUtils;

@SuppressWarnings("rawtypes")
public final class WCFExceptionHandler
{
	private static java.lang.Class[] _knowns = new java.lang.Class[] { ConnectException.class, ConnectIOException.class, NotBoundException.class, NoSuchObjectException.class, UnknownHostException.class,  CallbackExpiredException.class };


	public static boolean canCatch(Throwable error)
	{
		java.lang.Class et = error.getClass();
		return ArrayUtils.indexOf(_knowns, et) >= 0;

	}
}