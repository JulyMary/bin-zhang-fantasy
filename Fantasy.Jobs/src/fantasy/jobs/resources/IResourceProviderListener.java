package fantasy.jobs.resources;

import java.util.*;

public interface IResourceProviderListener {
    void Available(EventObject e);
    void Revoke(ProviderRevokeArgs e) throws Exception;
}
