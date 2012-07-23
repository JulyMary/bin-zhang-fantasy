package fantasy.jobs.resources;

import java.util.UUID;

public interface IResourceManagerHandler
{
	void Revoke(UUID id);

	UUID Id();
}