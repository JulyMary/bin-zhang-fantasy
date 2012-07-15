package Fantasy.ServiceModel;

public class ClientRef<T> implements IDisposable
{

	public ClientRef(T client)
	{
		this.setClient(client);
	}



	public final void dispose()
	{

		if (this.getState() <= CommunicationState.Opened)
		{
			try
			{
				((IChannel)this.Client).Close();
			}
			catch (java.lang.Exception e)
			{
			}
		}
	}


	public final CommunicationState getState()
	{
		return ((IChannel)this.Client).State;
	}


	private T privateClient;
	public final T getClient()
	{
		return privateClient;
	}
	private void setClient(T value)
	{
		privateClient = value;
	}



}