package fantasy.collections;

import java.util.Comparator;

import org.apache.commons.collections.ComparatorUtils;

public class NaturalComparator<T> implements Comparator<T> {

	@SuppressWarnings({ "unchecked" })
	@Override
	public int compare(T arg0, T arg1) {
		
		return ComparatorUtils.naturalComparator().compare(arg0, arg1);
	
	}

}
