package Fantasy.ServiceModel;

import Fantasy.Properties.*;

public class MissingRequiredServiceException extends RuntimeException implements Serializable
{
	public MissingRequiredServiceException(java.lang.Class serviceType)
	{
		super(String.format(Resources.getMissingRequiredServiceText(), serviceType));

	}
}