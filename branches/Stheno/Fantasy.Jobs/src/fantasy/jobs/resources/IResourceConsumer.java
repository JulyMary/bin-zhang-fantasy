package fantasy.jobs.resources;

import java.util.UUID;

public interface IResourceConsumer
{
	void Revoke(UUID id) throws Exception;

	UUID Id();
}