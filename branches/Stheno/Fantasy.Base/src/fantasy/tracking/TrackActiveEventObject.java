package fantasy.tracking;

import java.util.*;

public class TrackActiveEventObject extends EventObject {

	
	/**
	 * 
	 */
	private static final long serialVersionUID = -2904984358630834105L;
	TrackActiveEventObject(Object source, TrackMetaData track)
	{
		super(source);
		this._metaData = track;
	}
	
	private TrackMetaData _metaData;
	public TrackMetaData getMetaData()
	{
		return _metaData;
	}
}
