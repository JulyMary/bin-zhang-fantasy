package fantasy.servicemodel;

public final class Log
{
	public static void SafeLogMessage(ILogger logger, String category, MessageImportance importance, String message, Object... messageArgs)
	{
		if (logger != null)
		{
			logger.LogMessage(category, importance, String.format(message, messageArgs));
		}
	}
	public static void SafeLogMessage(ILogger logger, String category, String message, Object... messageArgs)
	{
		if (logger != null)
		{
			logger.LogMessage(category, String.format(message, messageArgs));
		}
	}
	public static void SafeLogWarning(ILogger logger, String category, Throwable exception, MessageImportance importance, String message, Object... messageArgs)
	{
		if (logger != null)
		{
			logger.LogWarning(category, exception, importance, String.format(message, messageArgs));
		}
	}
	public static void SafeLogWarning(ILogger logger, String category, MessageImportance importance, String message, Object... messageArgs)
	{
		if (logger != null)
		{
			logger.LogWarning(category, importance, String.format(message, messageArgs));
		}
	}
	public static void SafeLogWarning(ILogger logger, String category, String message, Object... messageArgs)
	{
		if (logger != null)
		{
			logger.LogWarning(category, String.format(message, messageArgs));
		}
	}
	public static void SafeLogError(ILogger logger, String category, Throwable exception, String message, Object... messageArgs)
	{
		if (logger != null)
		{
			logger.LogError(category, exception, String.format(message, messageArgs));
		}
	}
	public static void SafeLogError(ILogger logger, String category, String message, Object... messageArgs)
	{
		if (logger != null)
		{
			logger.LogError(category, String.format(message, messageArgs));
		}
	}
}