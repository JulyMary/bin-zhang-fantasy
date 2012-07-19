package fantasy.jobs;

public class JobExitEventArgs implements Serializable
{
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