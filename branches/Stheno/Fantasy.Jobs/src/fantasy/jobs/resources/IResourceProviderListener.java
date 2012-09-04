package fantasy.jobs.resources;

import java.util.*;

public interface IResourceProviderListener {
    void Available(EventObject e) throws Exception;
    void Revoke(ProviderRevokeArgs e) throws Exception;
}
