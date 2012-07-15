package Fantasy.Jobs.Resources;

public class ProviderRevokeArgs extends EventArgs
{
	private Object privateResource;
	public final Object getResource()
	{
		return privateResource;
	}
	public final void setResource(Object value)
	{
		privateResource = value;
	}
}