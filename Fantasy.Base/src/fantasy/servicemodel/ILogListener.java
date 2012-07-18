package fantasy.servicemodel;

public interface ILogListener
{
	void onMessage(String category, MessageImportance importance, String message) throws Exception;
	void onWaring(String category, Throwable exception, MessageImportance importance, String message) throws Exception;
	void onError(String category, Throwable exception, String message) throws Exception;
}