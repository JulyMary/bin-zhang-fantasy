package Fantasy;

public final class CollectionExtensions
{
//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static void SortBy<TSource, TKey>(this List<TSource> source, Func<TSource, TKey> keySelector)
	public static <TSource, TKey> void SortBy(java.util.ArrayList<TSource> source, Func<TSource, TKey> keySelector)
	{
		CollectionExtensions.<TSource, TKey>SortBy(source, keySelector, Comparer.Default);
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static int BinarySearchBy<TSource, TKey>(this List<TSource> source, TKey key, Func<TSource, TKey> keySelector)
	public static <TSource, TKey> int BinarySearchBy(java.util.ArrayList<TSource> source, TKey key, Func<TSource, TKey> keySelector)
	{
		return CollectionExtensions.<TSource, TKey>BinarySearchBy(source, key, keySelector, Comparer.Default);
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static void SortBy<TSource, TKey>(this List<TSource> source, Func<TSource, TKey> keySelector, IComparer comparer)
	public static <TSource, TKey> void SortBy(java.util.ArrayList<TSource> source, Func<TSource, TKey> keySelector, java.util.Comparator comparer)
	{
		if (comparer == null)
		{
			throw new ArgumentNullException("comparer");
		}

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Comparison<TSource> comparison = (x, y) =>
		{
			Object k1 = keySelector(x);
			Object k2 = keySelector(y);
			return comparer.Compare(k1, k2);
		}

		source.Sort(comparison);
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static int BinarySearchBy<TSource, TKey>(this List<TSource> source, TKey key, Func<TSource, TKey> keySelector, IComparer comparer)
	public static <TSource, TKey> int BinarySearchBy(java.util.ArrayList<TSource> source, TKey key, Func<TSource, TKey> keySelector, java.util.Comparator comparer)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Func<Integer, Object> valueSelector = (index) =>
			{
				return keySelector(source.get(index));
			}
		return InnerBinarySearchBy(0, source.size(), key, valueSelector, comparer);
	}


//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static void SortBy<TSource, TKey>(this Collection<TSource> source, Func<TSource, TKey> keySelector)
	public static <TSource, TKey> void SortBy(Collection<TSource> source, Func<TSource, TKey> keySelector)
	{
		CollectionExtensions.<TSource, TKey>SortBy(source, keySelector, Comparer.Default);
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static int BinarySearchBy<TSource, TKey>(this Collection<TSource> source, TKey key, Func<TSource, TKey> keySelector)
	public static <TSource, TKey> int BinarySearchBy(Collection<TSource> source, TKey key, Func<TSource, TKey> keySelector)
	{
		return CollectionExtensions.<TSource, TKey>BinarySearchBy(source, key, keySelector, Comparer.Default);
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static void SortBy<TSource, TKey>(this Collection<TSource> source, Func<TSource, TKey> keySelector, IComparer comparer)
	public static <TSource, TKey> void SortBy(Collection<TSource> source, Func<TSource, TKey> keySelector, java.util.Comparator comparer)
	{
		if (comparer == null)
		{
			throw new ArgumentNullException("comparer");
		}

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Comparison<TSource> comparison = (x, y) =>
		{
			Object k1 = keySelector(x);
			Object k2 = keySelector(y);
			return comparer.Compare(k1, k2);
		}

		source.getInnerList().Sort(comparison);
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static int BinarySearchBy<TSource, TKey>(this Collection<TSource> source, TKey key, Func<TSource, TKey> keySelector, IComparer comparer)
	public static <TSource, TKey> int BinarySearchBy(Collection<TSource> source, TKey key, Func<TSource, TKey> keySelector, java.util.Comparator comparer)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Func<Integer, Object> valueSelector = (index) =>
		{
			return keySelector(source.getItem(index));
		}
		return InnerBinarySearchBy(0, source.getCount(), key, valueSelector, comparer);
	}

	private static int InnerBinarySearchBy(int index, int count, Object key, Func<Integer, Object> valueSelector, java.util.Comparator comparer)
	{
		int low = index;
		int high = index + count - 1;
		while (low <= high)
		{
			int middle = low + ((high - low) >> 1);
			Object value = valueSelector(middle);
			int cmp = comparer.Compare(value, key);
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


//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static IEnumerable<T> Flatten<T>(this T item, Func<T, IEnumerable<T>> children, Func<T, bool> condition)
	public static <T> Iterable<T> Flatten(T item, Func<T, Iterable<T>> children, Func<T, Boolean> condition)
	{

			if (condition == null || condition(item))
			{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
				yield return item;
				if (children != null)
				{
					for(T child : children(item))
					{
						for(T descendant : child.Flatten(children, condition))
						{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
							yield return descendant;
						}
					}
				}
			}

	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static IEnumerable<T> Flatten<T>(this T item, Func<T, IEnumerable<T>> children)
	public static <T> Iterable<T> Flatten(T item, Func<T, Iterable<T>> children)
	{

		return Flatten(item, children, null);

	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static IEnumerable<T> Flatten<T>(this T item, Func<T, T> parent, Func<T, bool> condition)
	public static <T> Iterable<T> Flatten(T item, Func<T, T> parent, Func<T, Boolean> condition)
	{
		T obj = item;
		if(condition == null || condition(obj))
		{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
			yield return obj;
			T p = parent(obj);
			if (p != null)
			{
				for (T ancestor : Flatten(p, parent, condition))
				{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
					yield return ancestor;
				}
			}
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static IEnumerable<T> Flatten<T>(this T item, Func<T, T> parent)
	public static <T> Iterable<T> Flatten(T item, Func<T, T> parent)
	{
		return Flatten(item, parent, null);
	}

//C# TO JAVA CONVERTER TODO TASK: Extension methods are not available in Java:
//ORIGINAL LINE: public static Collection<T> ToCollection<T>(this IEnumerable<T> collection)
	public static <T> Collection<T> ToCollection(Iterable<T> collection)
	{
		return new Collection<T>(collection);
	}
}