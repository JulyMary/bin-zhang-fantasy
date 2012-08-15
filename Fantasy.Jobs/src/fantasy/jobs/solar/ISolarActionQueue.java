package fantasy.jobs.solar;

import fantasy.*;

public interface ISolarActionQueue
{
	void Enqueue(Action1<ISolar> action);
}