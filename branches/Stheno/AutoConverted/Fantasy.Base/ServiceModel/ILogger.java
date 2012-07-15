package Fantasy.ServiceModel;

public interface ILogger
{
	void LogMessage(String category, MessageImportance importance, String message, Object... messageArgs);
	void LogMessage(String category, String message, Object... messageArgs);
	void LogWarning(String category, RuntimeException exception, MessageImportance importance, String message, Object... messageArgs);
	void LogWarning(String category, MessageImportance importance, String message, Object... messageArgs);
	void LogWarning(String category, String message, Object... messageArgs);
	void LogError(String category, RuntimeException exception, String message, Object... messageArgs);
	void LogError(String category, String message, Object... messageArgs);

	void AddListener(ILogListener listener);
	void RemoveListener(ILogListener listener);

}