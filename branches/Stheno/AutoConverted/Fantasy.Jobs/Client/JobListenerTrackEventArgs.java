package Fantasy.Jobs.Client;

import Fantasy.Jobs.Management.*;
import Fantasy.ServiceModel.*;
import Fantasy.Tracking.*;

public class JobListenerTrackEventArgs extends EventArgs
{

	public JobListenerTrackEventArgs(ITrackListener track, TrackChangedEventArgs e)
	{
		this.setTrack(track);
		if (e != null)
		{
			setName(e.getName());
			setOldValue(e.OldValue);
			setNewValue(e.NewValue);
		}
	}

	private ITrackListener privateTrack;
	public final ITrackListener getTrack()
	{
		return privateTrack;
	}
	private void setTrack(ITrackListener value)
	{
		privateTrack = value;
	}

	public final Guid getId()
	{
		return getTrack().Id;
	}

	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	private void setName(String value)
	{
		privateName = value;
	}
	private Object privateOldValue;
	public final Object getOldValue()
	{
		return privateOldValue;
	}
	private void setOldValue(Object value)
	{
		privateOldValue = value;
	}
	private Object privateNewValue;
	public final Object getNewValue()
	{
		return privateNewValue;
	}
	private void setNewValue(Object value)
	{
		privateNewValue = value;
	}
}