package fantasy.jobs;

public interface IStateBag extends Iterable<StateBagItem>
{
Object getItem(String name);
	void setItem(String name, Object value);

	int getCount();

	String[] getNames();

	void Remove(String name);

	boolean ContainsState(String name);

	void Clear();
}