package fantasy.collections;

public interface IGrouping<TKey, TElement> extends Iterable<TElement>{

	TKey getKey();
	
	
	Enumerable<TElement> toEnumerable();

}
