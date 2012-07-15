package Fantasy.XSerialization;

public interface IXSerializable
{
	void Load(IServiceProvider context, XElement element);

	void Save(IServiceProvider context, XElement element);
}