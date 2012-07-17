package fantasy.tracking;

import java.util.EventListener;

public interface ITrackActiveEventListener extends EventListener {
    void HandleActive(TrackActiveEventObject e);
    
}
