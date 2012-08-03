package fantasy.collections;

import java.util.*;



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

		this._source = new ArrayIterable(source);
	}
	
	private class ArrayIterable implements Iterable<T>
	{
		public ArrayIterable(T[] source)
		{
			this._source = source;
		}

		@Override
		public Iterator<T> iterator() {
			
			return new ArrayIterator(this._source);
		}
		
		private T[] _source;
		
	}
	
	private class ArrayIterator implements Iterator<T>
	{
		public ArrayIterator(T[] source)
		{
			this._source = source;
		}
		
		private int _index = 0;

		private T[] _source;

		@Override
		public boolean hasNext() {
			return _index < this._source.length;
		}

		@Override
		public T next() {
			if(_index < this._source.length)
			{
				T rs = this._source[this._index];
				this._index ++;
				return rs;
			}
			else
			{
				throw new NoSuchElementException();
			}
		}

		@Override
		public void remove() {
			throw new UnsupportedOperationException();
			
		}
		
	}

	public <TResult> Enumerable<TResult> select(Selector<T, TResult> selector)
	{
		Iterable<TResult> rs = new SelectIterable<TResult>(this._source, selector);
		return new Enumerable<TResult>(rs);

	}
	
	
	private class SelectIterable<TResult> implements Iterable<TResult>
	{
		public SelectIterable(Iterable<T> source, Selector<T, TResult> selector)
		{
			this._source = source;
			this._selector = selector;
		}
		
		private Iterable<T> _source;
		private Selector<T, TResult> _selector;
		@Override
		public Iterator<TResult> iterator() {
			return new SelectIterator<TResult>(this._source, this._selector);
		}
	}
	
	private class SelectIterator<TResult> implements Iterator<TResult>
	{
		public SelectIterator(Iterable<T> source, Selector<T, TResult> selector)
		{
			this._source = source.iterator();
			this._selector = selector;
		}
		
		private Iterator<T> _source;
		private Selector<T, TResult> _selector;
		@Override
		public boolean hasNext() {
			return _source.hasNext();
		}
		@Override
		public TResult next() {
			T item = this._source.next();
			TResult rs = this._selector.select(item);
			return rs;
		}
		@Override
		public void remove() {
			throw new UnsupportedOperationException();
			
		}
		
	}
	


	public Enumerable<T> distinct()
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

		WhereIterable rs = new WhereIterable(this._source, predicate);
		
		return new Enumerable<T>(rs);
	}
	
	
	private class WhereIterable implements Iterable<T>
	{
		
		public WhereIterable(Iterable<T> source, Predicate<T> predicate)
		{
			this._predicate = predicate;
			this._source = source;
		}
		
		private Iterable<T> _source;
		private Predicate<T> _predicate;
		

		@Override
		public Iterator<T> iterator() {
			return new WhereIterator(this._source, this._predicate);
		}
		
	}
	
	private class WhereIterator implements Iterator<T>
	{
		
		public WhereIterator(Iterable<T> source, Predicate<T> predicate)
		{
			this._predicate = predicate;
			this._source = source.iterator();
		}
		
		private Iterator<T> _source;
		private Predicate<T> _predicate;
		private T _current;
		
		
		//0: need To Find Next, 1: return current, 2 : end 
		private int _state = 0;


		@Override
		public boolean hasNext() {
			if(_state == 0)
			{
				this.findNext();
			}
			
			return _state != 2;
		}


		private void findNext() throws UnsupportedOperationException {
			while(this._state == 0)
			{
				if(this._source.hasNext())
				{
					T item = this._source.next();
					try {
						if(this._predicate.evaluate(item))
						{
							this._current = item;
							this._state = 1;
						}
					} catch (Exception e) {
						throw new UnsupportedOperationException(e);
					}
					
				}
				else	
				{
					this._state = 2;
				}
			}
			
		}


		@Override
		public T next() {
			if(_state == 0)
			{
				this.findNext();
			}
			if(_state == 2)
			{
				throw new NoSuchElementException();
			}
			T rs = this._current;
			this._state = 0;
			return rs;
		}


		@Override
		public void remove() {
			throw new UnsupportedOperationException();
			
		}
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




	public <T2> Enumerable<T2> ofType(final Class<T2> clazz) throws Exception
	{
		
		return this.where(new Predicate<T>(){

			@Override
			public boolean evaluate(T obj) throws Exception {
				
				return clazz.isInstance(obj);
			}}).cast(clazz);
		
	}

	@SuppressWarnings("unchecked")
	public <T2> Enumerable<T2> cast(Class<T2> t2)
	{
		

		return this.select(new Selector<T, T2>(){

			@Override
			public T2 select(T item) {
				return (T2)item;
			}});

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

	public  T singleOrDefault(Predicate<T> predicate) throws Exception
	{
		T rs = null;
		for(T item : this._source)
		{
			if(predicate.evaluate(item))
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

	
	public <TKey> Enumerable<T> orderByDescending(Selector<T, TKey> keySelector)
	{
		return this.orderByDescending(keySelector, null);
	}
	
	public <TKey> Enumerable<T> orderByDescending(Selector<T, TKey> keySelector, Comparator<TKey> comparator)
	{
		KeyComparator<TKey> kc = new KeyComparator<TKey>();
		kc.keySelector = keySelector;
		kc.innerComparator = comparator != null ? comparator : new NaturalComparator<TKey>(); 
		
		OrderByIterable rs = new OrderByIterable(this._source, new ReverseComparator<T>(kc));
		return new Enumerable<T>(rs);
	}
	
	public <TKey> Enumerable<T> orderBy(Selector<T, TKey> keySelector)
	{
		return this.orderBy(keySelector, null);
	}

	public <TKey> Enumerable<T> orderBy(Selector<T, TKey> keySelector, Comparator<TKey> comparator)
	{
		
		KeyComparator<TKey> kc = new KeyComparator<TKey>();
		kc.keySelector = keySelector;
		kc.innerComparator = comparator != null ? comparator : new NaturalComparator<TKey>(); 

		OrderByIterable rs = new OrderByIterable(this._source, kc);

		return new Enumerable<T>(rs);
	}
	
	
	private class OrderByIterable implements Iterable<T>
	{
		
		public OrderByIterable(Iterable<T> source, Comparator<T> comparator)
		{
			this._source = source;
			this._comparer = comparator;
		}
		
		private Iterable<T> _source;
		private Comparator<T> _comparer;

		@Override
		public Iterator<T> iterator() {
			return new OrderByIterator(this._source, this._comparer);
		}
		
	}
	
	private class OrderByIterator implements Iterator<T>
	{
		
		public OrderByIterator(Iterable<T> source, Comparator<T> comparator)
		{
			_list = new ArrayList<T>();
			for(T item : source)
			{
				_list.add(item);
			}
			this._comparer = comparator;
		}
		
		
		private ArrayList<T> _list;
		
		private int _index = 0;
		private Comparator<T> _comparer;
		@Override
		public boolean hasNext() {
			
			return _index < _list.size();
		}
		@Override
		public T next() {
			if(_index >= _list.size())
			{
				throw new NoSuchElementException();
			}
			
			T rs;
			for(int i = this._list.size() - 1; i > this._index ; i --)
			{
				T x = this._list.get(i);
				T y = this._list.get(i - 1);
				if(this._comparer.compare(x,  y) < 0)
				{
					this._list.set(i, y);
					this._list.set(i - 1, x);
				}
			}
			
			rs = this._list.get(this._index);
			this._index ++;
			return rs;
			
		}
		@Override
		public void remove() {
			throw new UnsupportedOperationException();
			
		}

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
		Iterable<T2> rs = new FromIterable<T2>(this._source, selector);
		
		return new Enumerable<T2>(rs);
	}
	
	private class FromIterable<T2> implements Iterable<T2>
	{
		public FromIterable(Iterable<T> source, Selector<T, Iterable<T2>> selector)
		{
			this._source = source;
			this._selector = selector;
		}
		
		private Iterable<T> _source;
		private Selector<T, Iterable<T2>> _selector;

		@Override
		public Iterator<T2> iterator() {
			return new FromIterator<T2>(this._source, this._selector);
		}
	}
	
	
	private class FromIterator<T2> implements Iterator<T2>
	{
		public FromIterator(Iterable<T> source, Selector<T, Iterable<T2>> selector)
		{
			this._source = source.iterator();
			this._selector = selector;
		}
		
		private Iterator<T> _source;
		private Iterator<T2> _currentIterator;
		private Selector<T, Iterable<T2>> _selector;
		//0: need to call find next; 1: has item; 2 : end
		private int _state = 0;
		private T2 _current;
		
		@Override
		public boolean hasNext() {
			if(this._state == 0)
			{
				this.findNext();
			}
			return this._state != 2;
		}
		private void findNext() {
			while((_currentIterator == null || !_currentIterator.hasNext()) && this._state != 2)
			{
				if(_source.hasNext())
				{
					_currentIterator = _selector.select(_source.next()).iterator();
				}
				else
				{
					this._state = 2;
				}
			}
			
			if(this._state != 2)
			{
				this._current = this._currentIterator.next();
			}
			
		}
		@Override
		public T2 next() {
			if(_state == 0)
			{
				this.findNext();
			}
			if(_state == 2)
			{
				throw new NoSuchElementException();
			}
			T2 rs = this._current;
			this._state = 0;
			return rs;
		}
		@Override
		public void remove() {
			throw new UnsupportedOperationException();
			
		}

	
	}



	@Override
	public Iterator<T> iterator() {
		return this._source.iterator();
	}






}


