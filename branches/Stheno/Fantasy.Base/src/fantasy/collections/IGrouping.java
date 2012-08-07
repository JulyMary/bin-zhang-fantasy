package fantasy.collections;

public interface IGrouping<TKey, TElement> extends Iterable<TElement>, ICountable {

	TKey getKey();
	int count();
	
	Enumerable<TElement> toEnumerable();

}
