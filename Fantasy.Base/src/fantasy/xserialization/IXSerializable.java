package fantasy.xserialization;

import org.jdom2.*;

import fantasy.IServiceProvider;

public interface IXSerializable
{
	public void Load(IServiceProvider context, Element element);

	public void Save(IServiceProvider context, Element element);
}