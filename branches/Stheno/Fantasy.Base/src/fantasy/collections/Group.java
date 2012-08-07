package fantasy.collections;

import java.util.*;


class Group<TKey, TElement> implements IGrouping<TKey, TElement>{
	
	
	public Group(TKey key)
	{
		this._key = key;
	}
	
	@Override
	public Iterator<TElement> iterator() {
		return this.elements.iterator();
	}

	private TKey _key;
	@Override
	public TKey getKey() {
		
		return _key;
	}
	
	
	ArrayList<TElement> elements = new ArrayList<TElement>();
	@Override
	public int count() {
		return elements.size();
	}

	@Override
	public Enumerable<TElement> toEnumerable() {
		return new Enumerable<TElement>(this.elements);
	}

    
}
