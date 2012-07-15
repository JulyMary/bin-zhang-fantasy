package Fantasy.Jobs;

import Fantasy.XSerialization.*;

public abstract class AbstractInstruction implements IInstruction, IObjectWithSite
{

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IInstructment Members

	public abstract void Execute();

	private IServiceProvider privateSite;
	public final IServiceProvider getSite()
	{
		return privateSite;
	}
	public final void setSite(IServiceProvider value)
	{
		privateSite = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IConditionalObject Members


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion



}