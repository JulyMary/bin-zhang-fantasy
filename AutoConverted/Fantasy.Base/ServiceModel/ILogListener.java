package Fantasy.ServiceModel;

public interface ILogListener
{
	void OnMessage(String category, MessageImportance importance, String message);
	void OnWaring(String category, RuntimeException exception, MessageImportance importance, String message);
	void OnError(String category, RuntimeException exception, String message);
}