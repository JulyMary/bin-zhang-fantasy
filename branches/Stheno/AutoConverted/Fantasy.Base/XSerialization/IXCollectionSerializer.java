package Fantasy.XSerialization;

public interface IXCollectionSerializer
{
	void Save(IServiceProvider context, XElement element, Iterable collection);
	Iterable Load(IServiceProvider context, XElement element);
}