package fantasy.collections;

import java.util.*;

public class ReverseComparator<T> implements  Comparator<T>{

	public ReverseComparator(Comparator<T> comparator)
	{
		this._comparator = comparator;
	}
	
	public ReverseComparator()
	{
		this._comparator = new NaturalComparator<T>();
	}
	
	private Comparator<T> _comparator;
	
	@Override
	public int compare(T o1, T o2) {
		return -1 * this._comparator.compare(o1, o2);
	}

}
