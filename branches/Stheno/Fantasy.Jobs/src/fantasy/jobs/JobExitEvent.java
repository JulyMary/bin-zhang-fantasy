package fantasy.jobs;

import java.io.Serializable;
import java.util.*;

public class JobExitEvent extends EventObject implements Serializable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -619657948254416300L;
	public JobExitEvent(Object source, int state)
	{
		super(source);
		this.setExitState(state);
	}
	private int privateExitState;
	public final int getExitState()
	{
		return privateExitState;
	}
	private void setExitState(int value)
	{
		privateExitState = value;
	}
}