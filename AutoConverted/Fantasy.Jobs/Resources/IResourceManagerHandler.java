package Fantasy.Jobs.Resources;

public interface IResourceManagerHandler
{
	void Revoke(Guid id);

	Guid Id();
}