package fantasy.jobs;

import org.jdom2.*;

class NamespaceManager implements INamespaceManager {

	public Namespace[] getNamespaces()
	{
		return _namespace.clone();
	}
	
	public void setNamespaces(Namespace[] value)
	{
		_namespace = value;
	}
	
	private Namespace[] _namespace;
	
}
