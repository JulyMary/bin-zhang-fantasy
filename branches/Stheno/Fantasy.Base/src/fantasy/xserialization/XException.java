package fantasy.xserialization;

public class XException extends RuntimeException
{

	/**
	 * 
	 */
	private static final long serialVersionUID = -9087796539035574197L;

	
	
	public XException()
	{
	   super();
	}
	
	public XException(String message)
	{
		super(message);
	}
    
	public XException(String message, Throwable cause)
	{
	    super(message, cause);
	}
	
}