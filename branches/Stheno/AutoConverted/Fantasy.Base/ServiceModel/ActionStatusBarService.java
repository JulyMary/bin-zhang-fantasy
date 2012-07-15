package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;

public class ActionStatusBarService implements IStatusBarService
{
	private Action<String> _action;
	public ActionStatusBarService(Action<String> action)
	{
		this._action = action;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IStatusBarService Members

	public final void SetStatus(String status)
	{
		this._action(status);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}