package fantasy.collections;

import java.util.*;



@SuppressWarnings("rawtypes")
public  class Enumerable<T> implements Iterable<T> {


	private Iterable<T> _source;

	public Enumerable(Iterable<T> source)
	{
		if(source == null){
			throw new IllegalArgumentException("source");
		}
		this._source = source;
	}

	public Enumerable(T[] source)
	{
		if(source == null){
			throw new IllegalArgumentException("source");
		}

		this._source = Arrays.asList(source);
	}

	public <TResult> Enumerable<TResult> select(Selector<T, TResult> selector)
	{
		ArrayList<TResult> rs = new ArrayList<TResult>();
		for(T item : _source)
		{

			rs.add(selector.select(item));

		}

		return new Enumerable<TResult>(rs);

	}


	public Enumerable<T> Distinct()
	{
		HashSet<T> rs = new HashSet<T>();
		for(T item : this._source)
		{
			rs.add(item);
		}
		return new Enumerable<T>(rs);
	}


	public Enumerable<T> where(Predicate<T> predicate) throws Exception
	{
		ArrayList<T> rs = new ArrayList<T>();
		for(T item : _source)
		{
			if(predicate.evaluate(item))
			{
				rs.add(item);
			}


		}
		return new Enumerable<T>(rs);
	}


	public T firstOrDefault() throws Exception
	{
		return this.firstOrDefault(new TruePredicate<T>());
	}

	public  T firstOrDefault(Predicate<T> predicate) throws Exception
	{
		for(T item : this._source)
		{
			if(predicate.evaluate(item))
			{
				return item;
			}

		}
		return null;
	}


	static class TruePredicate<T2> implements  Predicate<T2>
	{

		@Override
		public boolean evaluate(T2 obj) throws Exception {
			return true;
		}



	}


	public T first() throws Exception
	{
		return this.first(new TruePredicate<T>());
	}

	public T first(Predicate<T> predicate) throws Exception
	{
		T rs = firstOrDefault(predicate);
		if(rs == null)
		{
			throw new IllegalStateException("No element in collection");
		}
		return null;

	}




	@SuppressWarnings("unchecked")
	public <T2> Enumerable<T2> ofType(Class<T2> clazz)
	{
		ArrayList<T2> rs = new ArrayList<T2>();
		for(Object item : this._source)
		{

			if(clazz.isInstance(item))
			{
				rs.add((T2)item);
			}
		}

		return new Enumerable<T2>(rs);
	}

	@SuppressWarnings("unchecked")
	public static <T2> Enumerable<T2> cast(Iterable source)
	{
		ArrayList<T2> rs = new ArrayList<T2>();
		for(Object item : source)
		{

			rs.add((T2)item);

		}

		return new Enumerable(rs);

	}

	public T single() throws Exception
	{
		return this.single(new TruePredicate<T>());
	}

	public T single(Predicate<T> predicate) throws Exception
	{
		T rs = singleOrDefault(predicate);

		if(rs == null)
		{
			throw new IllegalStateException("No elements match the predicate in collection");
		}

		return rs;
	}


	public T singleOrDefault() throws Exception
	{
		return this.singleOrDefault(new TruePredicate<T>());
	}

	public  T singleOrDefault(Predicate<T> predicate)
	{
		T rs = null;
		for(T item : this._source)
		{
			if(predicate.equals(item))
			{
				if(rs == null)
				{
					rs = item;
				}
				else	
				{ 
					throw new IllegalStateException("More than one elements match the predicate in collection");
				}
			}
		}



		return rs;
	}


	public ArrayList<T> toArrayList()
	{
		ArrayList<T> rs = new ArrayList<T>();
		for(T item : this._source)
		{

			rs.add(item);
		}
		return rs;
	}

	public <TKey> Enumerable<T> orderBy(Selector<T, TKey> keySelector)
	{
		return this.orderBy(keySelector, null);
	}

	public <TKey> Enumerable<T> orderBy(Selector<T, TKey> keySelector, Comparator<TKey> comparator)
	{
		ArrayList<T> rs = new ArrayList<>();
		for(T item : this._source)
		{
			rs.add(item);
		}


		KeyComparator<TKey> kc = new KeyComparator<TKey>();
		kc.keySelector = keySelector;
		kc.innerComparator = comparator != null ? comparator : new NaturalComparator<TKey>(); 

		Collections.sort(rs, kc);

		return new Enumerable<T>(rs);
	}

	private class KeyComparator<TKey> implements Comparator<T>
	{
		public Selector<T, TKey> keySelector;
		public Comparator<TKey> innerComparator;

		@Override
		public int compare(T o1, T o2) {
			TKey k1 = keySelector.select(o1);
			TKey k2 = keySelector.select(o2);

			return innerComparator.compare(k1, k2);
		}

	}
	
	
	public <T2> Enumerable<T2> from(Selector<T, Iterable<T2>> selector)
	{
		ArrayList<T2> rs = new ArrayList<T2>();
		for(T item : this._source)
		{
			for(T2 item2 : selector.select(item))
			{
				rs.add(item2);
			}
		}
		
		return new Enumerable<T2>(rs);
	}







	@Override
	public Iterator<T> iterator() {
		return this._source.iterator();
	}






}


