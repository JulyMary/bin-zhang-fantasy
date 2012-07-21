package fantasy.jobs;
import java.io.*;

public class JobTemplate implements Serializable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -5399663076529834657L;
	public JobTemplate()
	{
		this.setName("");
	}

	private int privateid;
	public final int getid()
	{
		return privateid;
	}
	public final void setid(int value)
	{
		privateid = value;
	}

	private String privateLocation;
	public final String getLocation()
	{
		return privateLocation;
	}
	public final void setLocation(String value)
	{
		privateLocation = value;
	}

	private String privateContent;
	public final String getContent()
	{
		return privateContent;
	}
	public final void setContent(String value)
	{
		privateContent = value;
	}

	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	public final void setName(String value)
	{
		privateName = value;
	}


	private boolean privateValid;
	public final boolean getValid()
	{
		return privateValid;
	}
	public final void setValid(boolean value)
	{
		privateValid = value;
	}

	public final String ToAbsolutPath(String path)
	{
		if (path == null)
		{
			throw new IllegalArgumentException("path");
		}
		
		return new java.io.File(path).getAbsolutePath();
	

	}
}