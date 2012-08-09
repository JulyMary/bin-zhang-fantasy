package fantasy.collections;

import java.util.*;


class Group<TKey, TElement> extends ArrayList<TElement> implements IGrouping<TKey, TElement>{
	
	
	/**
	 * 
	 */
	private static final long serialVersionUID = -3504824948822878657L;
	public Group(TKey key)
	{
		this._key = key;
	}
	
	

	private TKey _key;
	@Override
	public TKey getKey() {
		
		return _key;
	}
	
	


	@Override
	public Enumerable<TElement> toEnumerable() {
		return new Enumerable<TElement>(this);
	}

    
}
