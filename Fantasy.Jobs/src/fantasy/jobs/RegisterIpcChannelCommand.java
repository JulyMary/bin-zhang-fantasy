package fantasy.jobs;

public class RegisterIpcChannelCommand implements ICommand
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ICommand Members

	public final Object Execute(Object args)
	{
		java.util.Map props = new java.util.Hashtable();
		props.put("secure", true);
		props.put("tokenImpersonationLevel", "impersonation");
		props.put("authorizedGroup", "Everyone");
		props.put("portName", Process.GetCurrentProcess().ProcessName + ":" + Process.GetCurrentProcess().Id.toString());
		BinaryServerFormatterSinkProvider tempVar = new BinaryServerFormatterSinkProvider();
		tempVar.TypeFilterLevel = TypeFilterLevel.Full;
		IpcChannel ipc = new IpcChannel(props, new BinaryClientFormatterSinkProvider(), tempVar);
		ChannelServices.RegisterChannel(ipc, true);
		return null;

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}