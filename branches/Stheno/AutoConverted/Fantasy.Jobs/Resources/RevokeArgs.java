package Fantasy.Jobs.Resources;

public class RevokeArgs extends EventArgs
{
	public RevokeArgs()
	{
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