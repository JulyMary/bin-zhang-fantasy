package fantasy.jobs.resources;

import java.util.EventObject;

public class RevokeArgs extends EventObject
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -6319083483601834961L;
	public RevokeArgs(Object source)
	{
		super(source);
		
		this.setSuspendEngine(true);
	}

	private boolean privateSuspendEngine;
	public final boolean getSuspendEngine()
	{
		return privateSuspendEngine;
	}
	
	public final void setSuspendEngine(boolean value)
	{
		privateSuspendEngine = value;
	}
}