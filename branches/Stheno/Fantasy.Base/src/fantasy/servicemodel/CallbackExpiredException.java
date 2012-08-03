package fantasy.servicemodel;

public class CallbackExpiredException extends RuntimeException
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -6215782633571433993L;
	public CallbackExpiredException()
	{
		this.setMessage("Callback expired.");
	}

	private String privateMessage;
	public final String getMessage()
	{
		return privateMessage;
	}
	public final void setMessage(String value)
	{
		privateMessage = value;
	}
}