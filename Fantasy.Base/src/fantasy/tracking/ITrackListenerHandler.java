package fantasy.tracking;

import java.util.*;


public interface ITrackListenerHandler extends EventListener {
    void HandleChanged(TrackChangedEventObject e);
    void HandleActiveChanged(EventObject e);
    
}
