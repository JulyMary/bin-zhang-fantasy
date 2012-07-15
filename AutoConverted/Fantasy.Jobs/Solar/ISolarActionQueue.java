package Fantasy.Jobs.Solar;

public interface ISolarActionQueue
{
	void Enqueue(Action<ISolar> action);
}