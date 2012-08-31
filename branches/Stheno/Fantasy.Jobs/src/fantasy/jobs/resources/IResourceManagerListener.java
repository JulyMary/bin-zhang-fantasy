package fantasy.jobs.resources;

import java.util.EventObject;

public interface IResourceManagerListener {
    void available(EventObject e) throws Exception;
}
