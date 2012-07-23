package fantasy.xserialization;

import org.jdom2.*;

import fantasy.IServiceProvider;

@SuppressWarnings("rawtypes")
public interface IXCollectionSerializer
{
	public void Save(IServiceProvider context, Element element,  Iterable collection) throws Exception;
	
	Iterable Load(IServiceProvider context, Element element) throws Exception;
}