package Fantasy.ServiceModel;

public final class ILoggerExtension
{
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static void SafeLogMessage(this ILogger logger, string category, MessageImportance importance, string message, params object[] messageArgs)
	public static void SafeLogMessage(ILogger logger, String category, MessageImportance importance, String message, Object... messageArgs)
	{
		if (logger != null)
		{
			logger.LogMessage(category, importance, String.format(message, messageArgs));
		}
	}
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static void SafeLogMessage(this ILogger logger, string category, string message, params object[] messageArgs)
	public static void SafeLogMessage(ILogger logger, String category, String message, Object... messageArgs)
	{
		if (logger != null)
		{
			logger.LogMessage(category, String.format(message, messageArgs));
		}
	}
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static void SafeLogWarning(this ILogger logger, string category, Exception exception, MessageImportance importance, string message, params object[] messageArgs)
	public static void SafeLogWarning(ILogger logger, String category, RuntimeException exception, MessageImportance importance, String message, Object... messageArgs)
	{
		if (logger != null)
		{
			logger.LogWarning(category, exception, importance, String.format(message, messageArgs));
		}
	}
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static void SafeLogWarning(this ILogger logger, string category, MessageImportance importance, string message, params object[] messageArgs)
	public static void SafeLogWarning(ILogger logger, String category, MessageImportance importance, String message, Object... messageArgs)
	{
		if (logger != null)
		{
			logger.LogWarning(category, importance, String.format(message, messageArgs));
		}
	}
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static void SafeLogWarning(this ILogger logger, string category, string message, params object[] messageArgs)
	public static void SafeLogWarning(ILogger logger, String category, String message, Object... messageArgs)
	{
		if (logger != null)
		{
			logger.LogWarning(category, String.format(message, messageArgs));
		}
	}
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static void SafeLogError(this ILogger logger, string category, Exception exception, string message, params object[] messageArgs)
	public static void SafeLogError(ILogger logger, String category, RuntimeException exception, String message, Object... messageArgs)
	{
		if (logger != null)
		{
			logger.LogError(category, exception, String.format(message, messageArgs));
		}
	}
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static void SafeLogError(this ILogger logger, string category, string message, params object[] messageArgs)
	public static void SafeLogError(ILogger logger, String category, String message, Object... messageArgs)
	{
		if (logger != null)
		{
			logger.LogError(category, String.format(message, messageArgs));
		}
	}
}