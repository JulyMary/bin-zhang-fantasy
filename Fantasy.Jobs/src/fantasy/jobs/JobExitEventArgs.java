package fantasy.jobs;

import java.io.*;

public class JobExitEventArgs implements Serializable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 1874435862363789366L;
	public JobExitEventArgs(int state)
	{
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