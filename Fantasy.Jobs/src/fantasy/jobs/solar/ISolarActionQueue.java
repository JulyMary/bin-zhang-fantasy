package fantasy.jobs.solar;

import fantasy.*;

public interface ISolarActionQueue
{
	void enqueue(Action1<ISolar> action) throws Exception;
}