package Fantasy.Jobs.Management;

public class StartRemotingCommand implements ICommand
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ICommand Members

	public final Object Execute(Object args)
	{
		String configfile = Assembly.GetEntryAssembly().getLocation() + ".config";
		RemotingConfiguration.Configure(configfile, false);
		//bool allow = RemotingConfiguration.IsActivationAllowed(typeof(RemoteJobManagerAccessor));
		return null;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}