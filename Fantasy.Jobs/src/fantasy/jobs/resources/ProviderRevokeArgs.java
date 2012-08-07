package fantasy.jobs.resources;

import java.util.*;

public class ProviderRevokeArgs extends EventObject
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 8527606106353357271L;
	public ProviderRevokeArgs(Object source)
	{
		super(source);
	}
	
	private Object privateResource;
	public final Object getResource()
	{
		return privateResource;
	}
	public final void setResource(Object value)
	{
		privateResource = value;
	}
}