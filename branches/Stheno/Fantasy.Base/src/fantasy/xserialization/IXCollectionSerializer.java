package fantasy.xserialization;

import org.jdom2.*;

import fantasy.IServiceProvider;

public interface IXCollectionSerializer
{
	public void Save(IServiceProvider context, Element element, @SuppressWarnings("rawtypes") Iterable collection);
	@SuppressWarnings("rawtypes")
	Iterable Load(IServiceProvider context, Element element);
}