package fantasy.collections;

import java.util.*;

public class CollectionUtils {
    public static <T, TKey> int binarySearchBy(List<T> source, TKey key, Selector<T, TKey> selector, Comparator<TKey> comparator)
    {
    
    	
    	
    	int low = 0;
    	int high = source.size() - 1;
    	while (low <= high)
    	{
    		int middle = low + ((high - low) >> 1);
    		T item = source.get(middle);
    		TKey value = selector.select(item);
    		int cmp = comparator.compare(value, key);
    		if (cmp == 0)
    		{
    			return middle;
    		}
    		if (cmp < 0)
    		{
    			low = middle + 1;
    		}
    		else
    		{
    			high = middle - 1;
    		}
    	}
    	return ~low;
    	
    }
    
    public static <T, TKey> int binarySearchBy(List<T> source, TKey key, Selector<T, TKey> selector)
    {
    	return binarySearchBy(source, key, selector, new NaturalComparator<TKey>());
    }
}
