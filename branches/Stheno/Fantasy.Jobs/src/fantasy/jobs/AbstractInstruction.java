package fantasy.jobs;

import fantasy.*;

public abstract class AbstractInstruction extends ObjectWithSite implements IInstruction, IObjectWithSite
{
	public abstract void Execute();
}