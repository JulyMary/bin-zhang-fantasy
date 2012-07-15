package Fantasy.ServiceModel;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract]
public class CallbackExpiredException
{
	public CallbackExpiredException()
	{
		this.setMessage("Callback expired.");
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember]
	private String privateMessage;
	public final String getMessage()
	{
		return privateMessage;
	}
	public final void setMessage(String value)
	{
		privateMessage = value;
	}
}