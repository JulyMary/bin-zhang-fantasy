package fantasy.collections;

import java.util.Comparator;

public class NaturalComparator<T> implements Comparator<T> {

	@SuppressWarnings({ "rawtypes", "unchecked" })
	@Override
	public int compare(T arg0, T arg1) {
		
		return ((Comparable)arg0).compareTo(arg1);
	
	}

}
